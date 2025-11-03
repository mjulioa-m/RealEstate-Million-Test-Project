import { render, screen, fireEvent } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import FilterForm from "../app/components/FilterForm";

describe("FilterForm", () => {
  const mockSetFilters = jest.fn();

  const defaultFilters = {
    name: "",
    address: "",
    minPrice: "",
    maxPrice: "",
  };

  beforeEach(() => {
    mockSetFilters.mockClear();
  });

  it("renders all inputs and buttons", () => {
    render(<FilterForm filters={defaultFilters} setFilters={mockSetFilters} />);

    expect(screen.getByPlaceholderText("Nombre")).toBeInTheDocument();
    expect(screen.getByPlaceholderText("Dirección")).toBeInTheDocument();
    expect(screen.getByPlaceholderText("Precio mínimo")).toBeInTheDocument();
    expect(screen.getByPlaceholderText("Precio máximo")).toBeInTheDocument();
  });

  it("calls setFilters on input change", async () => {
    render(<FilterForm filters={defaultFilters} setFilters={mockSetFilters} />);

    const nameInput = screen.getByPlaceholderText("Nombre");
    fireEvent.change(nameInput, { target: { value: "Casa" } });

    expect(mockSetFilters).toHaveBeenCalledWith({
      ...defaultFilters,
      name: "Casa",
    });
  });
});
