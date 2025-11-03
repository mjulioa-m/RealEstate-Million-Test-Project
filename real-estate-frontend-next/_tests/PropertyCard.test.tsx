import { render, screen } from "@testing-library/react";
import "@testing-library/jest-dom";
import PropertyCard from "../app/components/PropertyCard";
import { Property } from "../app/types";
import { useRouter } from "next/navigation";
import Link from "next/link";

jest.mock("next/link", () => {
  return ({ href, children }: any) => <a href={href}>{children}</a>;
});

describe("PropertyCard", () => {
  const property: Property = {
    id: "123",
    idOwner: "owner1",
    name: "Casa Bonita",
    address: "Calle Falsa 123",
    price: 250000,
    imageUrl: "https://example.com/image.jpg",
  };

  it("renders property name, address, and price", () => {
    render(<PropertyCard property={property} />);

    expect(screen.getByText("Casa Bonita")).toBeInTheDocument();
    expect(screen.getByText("Calle Falsa 123")).toBeInTheDocument();
    expect(
      screen.getByText((content) => content.includes("250"))
    ).toBeInTheDocument();
  });

  it("renders the image with correct src and alt", () => {
    render(<PropertyCard property={property} />);
    const image = screen.getByRole("img");

    expect(image).toHaveAttribute("src", property.imageUrl);
    expect(image).toHaveAttribute("alt", property.name);
  });

  it("links to the property details page", () => {
    render(<PropertyCard property={property} />);
    const link = screen.getByRole("link");

    expect(link).toHaveAttribute("href", `/properties/${property.id}`);
  });
});
