import { describe, it, expect, vi } from "vitest";
import { render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { PropertyFilters } from "./PropertyFilters";
import type { PropertyFilter } from "../../lib/types/property.types";

const mockFilters: PropertyFilter = {
  name: "",
  address: "",
  minPrice: undefined,
  maxPrice: undefined,
};

describe("PropertyFilters", () => {
  it("should render all filter inputs", () => {
    const onFilterChange = vi.fn();
    const onReset = vi.fn();

    render(
      <PropertyFilters
        filters={mockFilters}
        onFilterChange={onFilterChange}
        onReset={onReset}
      />
    );

    expect(screen.getByLabelText("Property Name")).toBeInTheDocument();
    expect(screen.getByLabelText("Address")).toBeInTheDocument();
    expect(screen.getByLabelText("Minimum Price")).toBeInTheDocument();
    expect(screen.getByLabelText("Maximum Price")).toBeInTheDocument();
  });

  it("should render action buttons", () => {
    const onFilterChange = vi.fn();
    const onReset = vi.fn();

    render(
      <PropertyFilters
        filters={mockFilters}
        onFilterChange={onFilterChange}
        onReset={onReset}
      />
    );

    expect(screen.getByRole("button", { name: /Apply/i })).toBeInTheDocument();
    expect(screen.getByRole("button", { name: /Reset/i })).toBeInTheDocument();
  });

  it("should submit form with text filters", async () => {
    const user = userEvent.setup();
    const onFilterChange = vi.fn();
    const onReset = vi.fn();

    render(
      <PropertyFilters
        filters={mockFilters}
        onFilterChange={onFilterChange}
        onReset={onReset}
      />
    );

    const nameInput = screen.getByLabelText("Property Name");
    const addressInput = screen.getByLabelText("Address");

    await user.type(nameInput, "Villa");
    await user.type(addressInput, "Main St");

    const submitButton = screen.getByRole("button", { name: /Apply/i });
    await user.click(submitButton);

    await waitFor(() => {
      expect(onFilterChange).toHaveBeenCalledWith({
        name: "Villa",
        address: "Main St",
        minPrice: undefined,
        maxPrice: undefined,
      });
    });
  });

  it("should call onReset when reset button is clicked", async () => {
    const user = userEvent.setup();
    const onFilterChange = vi.fn();
    const onReset = vi.fn();

    render(
      <PropertyFilters
        filters={mockFilters}
        onFilterChange={onFilterChange}
        onReset={onReset}
      />
    );

    const resetButton = screen.getByRole("button", { name: /Reset/i });
    await user.click(resetButton);

    expect(onReset).toHaveBeenCalledTimes(1);
  });

  it("should validate minimum price is not negative", async () => {
    const user = userEvent.setup();
    const onFilterChange = vi.fn();
    const onReset = vi.fn();

    render(
      <PropertyFilters
        filters={mockFilters}
        onFilterChange={onFilterChange}
        onReset={onReset}
      />
    );

    const minPriceInput = screen.getByLabelText("Minimum Price");
    await user.type(minPriceInput, "-100");

    const submitButton = screen.getByRole("button", { name: /Apply/i });
    await user.click(submitButton);

    await waitFor(() => {
      expect(
        screen.getByText("Minimum price must be 0 or greater")
      ).toBeInTheDocument();
    });

    expect(onFilterChange).not.toHaveBeenCalled();
  });

  it("should validate maximum price is not negative", async () => {
    const user = userEvent.setup();
    const onFilterChange = vi.fn();
    const onReset = vi.fn();

    render(
      <PropertyFilters
        filters={mockFilters}
        onFilterChange={onFilterChange}
        onReset={onReset}
      />
    );

    const maxPriceInput = screen.getByLabelText("Maximum Price");
    await user.type(maxPriceInput, "-500");

    const submitButton = screen.getByRole("button", { name: /Apply/i });
    await user.click(submitButton);

    await waitFor(() => {
      expect(
        screen.getByText("Maximum price must be 0 or greater")
      ).toBeInTheDocument();
    });

    expect(onFilterChange).not.toHaveBeenCalled();
  });

  it("should validate min price is less than or equal to max price", async () => {
    const user = userEvent.setup();
    const onFilterChange = vi.fn();
    const onReset = vi.fn();

    render(
      <PropertyFilters
        filters={mockFilters}
        onFilterChange={onFilterChange}
        onReset={onReset}
      />
    );

    const minPriceInput = screen.getByLabelText("Minimum Price");
    const maxPriceInput = screen.getByLabelText("Maximum Price");

    await user.type(minPriceInput, "1000000");
    await user.type(maxPriceInput, "500000");

    const submitButton = screen.getByRole("button", { name: /Apply/i });
    await user.click(submitButton);

    await waitFor(() => {
      expect(
        screen.getByText(
          "Minimum price must be less than or equal to maximum price"
        )
      ).toBeInTheDocument();
    });

    expect(onFilterChange).not.toHaveBeenCalled();
  });

  it("should accept valid price range", async () => {
    const user = userEvent.setup();
    const onFilterChange = vi.fn();
    const onReset = vi.fn();

    render(
      <PropertyFilters
        filters={mockFilters}
        onFilterChange={onFilterChange}
        onReset={onReset}
      />
    );

    const minPriceInput = screen.getByLabelText("Minimum Price");
    const maxPriceInput = screen.getByLabelText("Maximum Price");

    await user.type(minPriceInput, "500000");
    await user.type(maxPriceInput, "1000000");

    const submitButton = screen.getByRole("button", { name: /Apply/i });
    await user.click(submitButton);

    await waitFor(() => {
      expect(onFilterChange).toHaveBeenCalledWith({
        name: undefined,
        address: undefined,
        minPrice: 500000,
        maxPrice: 1000000,
      });
    });
  });

  it("should clear form when reset is clicked", async () => {
    const user = userEvent.setup();
    const onFilterChange = vi.fn();
    const onReset = vi.fn();

    render(
      <PropertyFilters
        filters={mockFilters}
        onFilterChange={onFilterChange}
        onReset={onReset}
      />
    );

    const nameInput = screen.getByLabelText(
      "Property Name"
    ) as HTMLInputElement;
    await user.type(nameInput, "Villa");

    const resetButton = screen.getByRole("button", { name: /Reset/i });
    await user.click(resetButton);

    expect(nameInput.value).toBe("");
  });

  it("should populate form with initial filter values", () => {
    const onFilterChange = vi.fn();
    const onReset = vi.fn();
    const filtersWithValues: PropertyFilter = {
      name: "Villa",
      address: "Beverly Hills",
      minPrice: 500000,
      maxPrice: 1000000,
    };

    render(
      <PropertyFilters
        filters={filtersWithValues}
        onFilterChange={onFilterChange}
        onReset={onReset}
      />
    );

    const nameInput = screen.getByLabelText(
      "Property Name"
    ) as HTMLInputElement;
    const addressInput = screen.getByLabelText("Address") as HTMLInputElement;
    const minPriceInput = screen.getByLabelText(
      "Minimum Price"
    ) as HTMLInputElement;
    const maxPriceInput = screen.getByLabelText(
      "Maximum Price"
    ) as HTMLInputElement;

    expect(nameInput.value).toBe("Villa");
    expect(addressInput.value).toBe("Beverly Hills");
    expect(minPriceInput.value).toBe("500000");
    expect(maxPriceInput.value).toBe("1000000");
  });
});
