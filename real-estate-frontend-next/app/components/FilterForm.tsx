import "./styles.css";

interface FilterFormProps {
  filters: {
    name?: string;
    address?: string;
    minPrice?: string;
    maxPrice?: string;
  };
  setFilters: (filters: FilterFormProps["filters"]) => void;
}

export default function FilterForm({ filters, setFilters }: FilterFormProps) {
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFilters({ ...filters, [e.target.name]: e.target.value });
  };

  return (
    <div className="filter-container">
      <form role="form" className="filter-form">
        <input
          name="name"
          placeholder="Nombre"
          value={filters.name || ""}
          onChange={handleChange}
        />
        <input
          name="address"
          placeholder="Dirección"
          value={filters.address || ""}
          onChange={handleChange}
        />
        <input
          name="minPrice"
          placeholder="Precio mínimo"
          value={filters.minPrice || ""}
          onChange={handleChange}
        />
        <input
          name="maxPrice"
          placeholder="Precio máximo"
          value={filters.maxPrice || ""}
          onChange={handleChange}
        />
        {/* <button type="submit" className="btn-filter">
          Filtrar
        </button> */}
      </form>
    </div>
  );
}
