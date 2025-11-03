import { render, screen, waitFor } from "@testing-library/react";
import PropertyDetail from "../app/properties/[id]/page";
import "@testing-library/jest-dom";

jest.mock("next/navigation", () => ({
  useParams: () => ({ id: "123" }),
  useRouter: () => ({ back: jest.fn() }),
}));

global.fetch = jest.fn();
const originalError = console.error;

beforeAll(() => {
  console.error = (...args) => {
    const [firstArg] = args;

    const isIgnorable =
      (typeof firstArg === "string" &&
        (firstArg.includes("not wrapped in act") ||
          firstArg.includes("Propiedad no encontrada"))) ||
      (firstArg instanceof Error &&
        (firstArg.message.includes("not wrapped in act") ||
          firstArg.message.includes("Propiedad no encontrada")));

    if (isIgnorable) return;

    return originalError.apply(console, args);
  };
});

afterAll(() => {
  console.error = originalError;
});

describe("PropertyDetail Page", () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  it("muestra mensaje de carga inicialmente", () => {
    (fetch as jest.Mock).mockResolvedValueOnce({
      ok: true,
      json: async () => ({}),
    });

    render(<PropertyDetail />);
    expect(screen.getByText("Cargando propiedad...")).toBeInTheDocument();
  });

  it("muestra los datos de la propiedad correctamente", async () => {
    const fakeProperty = {
      id: 123,
      name: "Casa Bonita",
      address: "Calle Falsa 123",
      price: 250000,
      imageUrl: "https://example.com/image.jpg",
    };

    (fetch as jest.Mock).mockResolvedValueOnce({
      ok: true,
      json: async () => fakeProperty,
    });

    render(<PropertyDetail />);

    await waitFor(() =>
      expect(screen.getByText("Casa Bonita")).toBeInTheDocument()
    );

    expect(screen.getByText("Calle Falsa 123")).toBeInTheDocument();
    expect(
      screen.getByText((content) => content.includes("250"))
    ).toBeInTheDocument();
    expect(screen.getByRole("img")).toHaveAttribute(
      "src",
      "https://example.com/image.jpg"
    );
  });

  it("muestra mensaje de error si la propiedad no se encuentra", async () => {
    (fetch as jest.Mock).mockResolvedValueOnce({
      ok: false,
    });

    render(<PropertyDetail />);

    await waitFor(() =>
      expect(screen.getByText("Propiedad no encontrada")).toBeInTheDocument()
    );
  });
});
