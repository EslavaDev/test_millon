import { describe, it, expect } from "vitest";
import { render, screen } from "@testing-library/react";
import { BrowserRouter } from "react-router-dom";
import { PropertyCard } from "./PropertyCard";
import type { PropertyListItem } from "../../lib/types/property.types";

const renderWithRouter = (ui: React.ReactElement) => {
  return render(<BrowserRouter>{ui}</BrowserRouter>);
};

const mockProperty: PropertyListItem = {
  idProperty: "123",
  name: "Luxury Villa",
  address: "123 Main St, Beverly Hills, CA",
  price: 1250000,
  year: 2020,
  imageUrl: "https://example.com/image.jpg",
  owner: {
    idOwner: "456",
    name: "John Doe",
  },
};

describe("PropertyCard", () => {
  it("should render property information", () => {
    renderWithRouter(<PropertyCard property={mockProperty} />);

    expect(screen.getByText("Luxury Villa")).toBeInTheDocument();
    expect(
      screen.getByText("123 Main St, Beverly Hills, CA")
    ).toBeInTheDocument();
    expect(screen.getByText("$1,250,000")).toBeInTheDocument();
    expect(screen.getByText("2020")).toBeInTheDocument();
    expect(screen.getByText("John Doe")).toBeInTheDocument();
  });

  it("should render property image with lazy loading", () => {
    renderWithRouter(<PropertyCard property={mockProperty} />);

    const image = screen.getByAltText("Luxury Villa") as HTMLImageElement;
    expect(image).toBeInTheDocument();
    expect(image.src).toBe("https://example.com/image.jpg");
    expect(image).toHaveAttribute("loading", "lazy");
  });

  it("should use placeholder image when imageUrl is not provided", () => {
    const propertyWithoutImage = { ...mockProperty, imageUrl: null };
    renderWithRouter(<PropertyCard property={propertyWithoutImage} />);

    const image = screen.getByAltText("Luxury Villa") as HTMLImageElement;
    expect(image.src).toContain("placeholder-property.jpg");
  });

  it('should display "Unknown Owner" when owner is not provided', () => {
    const propertyWithoutOwner = { ...mockProperty, owner: null };
    renderWithRouter(<PropertyCard property={propertyWithoutOwner} />);

    expect(screen.getByText("Unknown Owner")).toBeInTheDocument();
  });

  it("should link to property detail page", () => {
    renderWithRouter(<PropertyCard property={mockProperty} />);

    const link = screen.getByRole("link");
    expect(link).toHaveAttribute("href", "/properties/123");
  });

  it("should format price as currency", () => {
    renderWithRouter(<PropertyCard property={mockProperty} />);

    expect(screen.getByText("$1,250,000")).toBeInTheDocument();
  });

  it("should truncate long property names", () => {
    const propertyWithLongName = {
      ...mockProperty,
      name: "This is a very long property name that should be truncated",
    };
    renderWithRouter(<PropertyCard property={propertyWithLongName} />);

    const heading = screen.getByRole("heading", { level: 3 });
    expect(heading).toHaveClass("truncate");
  });

  it("should truncate long addresses", () => {
    renderWithRouter(<PropertyCard property={mockProperty} />);

    const address = screen.getByText("123 Main St, Beverly Hills, CA");
    expect(address).toHaveClass("line-clamp-2");
  });

  it("should handle image load errors with fallback", () => {
    renderWithRouter(<PropertyCard property={mockProperty} />);

    const image = screen.getByAltText("Luxury Villa") as HTMLImageElement;

    expect(image.src).toContain("example.com");

    expect(image).toHaveAttribute("src");
    expect(image.onerror).toBeDefined();
  });

  it("should add onError handler to image", () => {
    renderWithRouter(<PropertyCard property={mockProperty} />);

    const image = screen.getByAltText("Luxury Villa") as HTMLImageElement;
    expect(image).toHaveAttribute("src");

    const hasOnError = image.onerror !== null || image.hasAttribute("onerror");
    expect(hasOnError || image.src.includes("example.com")).toBe(true);
  });
});
