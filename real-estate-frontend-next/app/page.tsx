"use client";

import { useState, useEffect } from "react";
import PropertyCard from "./components/PropertyCard";
import FilterForm from "./components/FilterForm";
import { Property } from "./types";
import { toast } from "react-hot-toast";

const apiUrl = process.env.NEXT_PUBLIC_API_URL;
const PAGE_SIZE = 20;

export default function Home() {
  const [properties, setProperties] = useState<Property[]>([]);
  const [filters, setFilters] = useState<Record<string, string>>({
    name: "",
    address: "",
    minPrice: "",
    maxPrice: "",
  });
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(false);
  const [hasMore, setHasMore] = useState(true);
  const fetchProperties = async (reset = false) => {
    setLoading(true);
    try {
      const currentFilters = {
        ...filters,
        page: page.toString(),
        pageSize: PAGE_SIZE.toString(),
      };

      const cleanedFilters: Record<string, string> = {};

      Object.entries(currentFilters).forEach(([key, value]) => {
        if (value) {
          cleanedFilters[key] = value;
        }
      });

      const query = new URLSearchParams(cleanedFilters);

      const res = await fetch(`${apiUrl}/properties?${query}`);
      if (!res.ok) {
        console.error("Error status:", res.status, res.statusText);
        toast.error("Error al buscar propiedades");

        throw new Error("Error fetching properties");
      }

      const data: Property[] = await res.json();

      if (reset) setProperties(data);
      else setProperties((prev) => [...prev, ...data]);

      setHasMore(data.length === PAGE_SIZE);
    } catch (error) {
      console.error(error);
      toast.error("Error de conexión");
    }
    setLoading(false);
  };
  useEffect(() => {
    fetchProperties(true);
  }, [filters]);

  const loadMore = () => {
    setPage((prev) => prev + 1);
  };

  useEffect(() => {
    if (page > 1) fetchProperties();
  }, [page]);
  const addTestProperties = async () => {
    try {
      const properties = Array.from({ length: 100 }, (_, i) => ({
        idOwner: `owner-${i + 1}`,
        name: `Propiedad ${i + 1}`,
        address: `Calle Falsa ${i + 1}, Ciudad`,
        price: Math.floor(Math.random() * 900000 + 100000),

        image: {
          file: `https://picsum.photos/seed/${i + 1}/400/300`,
          enabled: true,
        },
      }));

      const res = await fetch(`${apiUrl}/properties/batch`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(properties),
      });

      if (res.ok) {
        toast.success("100 propiedades agregadas!");
      } else {
        toast.error("Error al agregar propiedades");
      }
    } catch (error) {
      toast.error("Error de conexión");
    }
  };
  return (
    <div className="container">
      <h1>PROPIEDADES</h1>
      <div className="batch-button-container">
        <button type="button" onClick={addTestProperties} className="btn-batch">
          Agregar 100 propiedades de prueba
        </button>
      </div>
      <FilterForm filters={filters} setFilters={setFilters} />

      {properties.length === 0 && !loading && (
        <div className="no-results">No se encontraron propiedades 😔</div>
      )}

      <ul className="property-list">
        {properties.map((p) => (
          <li key={p.id}>
            <PropertyCard property={p} />
          </li>
        ))}
      </ul>

      {loading && <div className="loading">Cargando propiedades...</div>}

      {!loading && hasMore && (
        <div style={{ textAlign: "center", margin: "20px 0" }}>
          <button onClick={loadMore}>Cargar más</button>
        </div>
      )}
    </div>
  );
}
