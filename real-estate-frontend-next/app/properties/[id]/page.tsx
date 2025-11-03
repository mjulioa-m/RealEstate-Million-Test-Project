"use client";

import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { Property } from "../../types";
import "./../../components/styles.css";

export default function PropertyDetail() {
  const params = useParams();
  const router = useRouter();
  const { id } = params;
  const [property, setProperty] = useState<Property | null>(null);
  const [loading, setLoading] = useState(true);

  const apiUrl = process.env.NEXT_PUBLIC_API_URL;

  useEffect(() => {
    const fetchProperty = async () => {
      try {
        const res = await fetch(`${apiUrl}/properties/${id}`);
        if (!res.ok) throw new Error("Propiedad no encontrada");
        const data: Property = await res.json();
        setProperty(data);
      } catch (error) {
        console.error(error);
      } finally {
        setLoading(false);
      }
    };
    fetchProperty();
  }, [id]);

  if (loading)
    return <p style={{ textAlign: "center" }}>Cargando propiedad...</p>;
  if (!property)
    return <p style={{ textAlign: "center" }}>Propiedad no encontrada</p>;

  return (
    <div
      style={{
        maxWidth: 800,
        margin: "20px auto",
        padding: 20,
        backgroundColor: "#fff",
        borderRadius: 10,
        boxShadow: "0px 4px 15px rgba(0,0,0,0.1)",
        color: "black",
      }}
    >
      <img
        src={property.imageUrl}
        alt={property.name}
        style={{ width: "100%", borderRadius: 10, marginBottom: 20 }}
      />
      <h1 style={{ marginBottom: 10 }}>{property.name}</h1>
      <p style={{ marginBottom: 5 }}>
        <strong>Dirección:</strong> {property.address}
      </p>
      <p style={{ marginBottom: 20 }}>
        <strong>Precio:</strong> ${property.price.toLocaleString()}
      </p>
      <button
        onClick={() => router.back()}
        style={{
          padding: "10px 20px",
          backgroundColor: "#0070f3",
          color: "#fff",
          border: "none",
          borderRadius: 5,
          cursor: "pointer",
        }}
      >
        Volver a la lista
      </button>
    </div>
  );
}
