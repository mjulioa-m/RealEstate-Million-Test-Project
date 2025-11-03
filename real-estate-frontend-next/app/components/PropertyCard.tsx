import Link from "next/link";
import { Property } from "../types";
import "./styles.css";

interface Props {
  property: Property;
}

export default function PropertyCard({ property }: Props) {
  return (
    <Link href={`/properties/${property.id}`}>
      <div className="card property-card">
        <div className="card-image">
          <img src={property.imageUrl} alt={property.name} />
        </div>
        <div className="card-content">
          <h2>{property.name}</h2>
          <p className="address">{property.address}</p>
          <p className="price">${property.price.toLocaleString()}</p>
        </div>
      </div>
    </Link>
  );
}
