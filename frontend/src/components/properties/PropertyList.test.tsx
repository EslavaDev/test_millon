import { describe, it, expect } from "vitest";
import { render, screen } from "@testing-library/react";
import { BrowserRouter } from "react-router-dom";
import { PropertyList } from "./PropertyList";
import type { PropertyListItem } from "../../lib/types/property.types";

const renderWithRouter = (ui: React.ReactElement) => {
  return render(<BrowserRouter>{ui}</BrowserRouter>);
};

const mockProperties: PropertyListItem[] = [
  {
    idProperty: "1",
    name: "Property 1",
    address: "123 Main St",
    price: 500000,
    year: 2020,
    imageUrl: "https://example.com/image1.jpg",
    owner: { idOwner: "1", name: "Owner 1" },
  },
  {
    idProperty: "2",
    name: "Property 2",
    address: "456 Oak Ave",
    price: 750000,
    year: 2021,
    imageUrl: "https://example.com/image2.jpg",
    owner: { idOwner: "2", name: "Owner 2" },
  },
];

describe("PropertyList", () => {
  it("should render loading state with skeleton loaders", () => {
    renderWithRouter(<PropertyList properties={[]} isLoading={true} />);

    const skeletons = screen
      .getAllByRole("generic")
      .filter((el) => el.className.includes("animate-pulse"));
    expect(skeletons.length).toBeGreaterThan(0);
  });

  it("should render error state", () => {
    renderWithRouter(
      <PropertyList
        properties={[]}
        isLoading={false}
        error="Failed to load properties"
      />
    );

    expect(screen.getByText("Error Loading Properties")).toBeInTheDocument();
    expect(screen.getByText("Failed to load properties")).toBeInTheDocument();
  });

  it("should render empty state when no properties", () => {
    renderWithRouter(<PropertyList properties={[]} isLoading={false} />);

    expect(screen.getByText("No Properties Found")).toBeInTheDocument();
    expect(
      screen.getByText("Try adjusting your filters to see more results.")
    ).toBeInTheDocument();
  });

  it("should render property cards when properties are provided", () => {
    renderWithRouter(
      <PropertyList properties={mockProperties} isLoading={false} />
    );

    expect(screen.getByText("Property 1")).toBeInTheDocument();
    expect(screen.getByText("Property 2")).toBeInTheDocument();
  });

  it("should render correct number of property cards", () => {
    renderWithRouter(
      <PropertyList properties={mockProperties} isLoading={false} />
    );

    const links = screen.getAllByRole("link");
    expect(links).toHaveLength(2);
  });

  it("should apply responsive grid classes", () => {
    const { container } = renderWithRouter(
      <PropertyList properties={mockProperties} isLoading={false} />
    );

    const grid = container.querySelector(".grid");
    expect(grid).toHaveClass("grid-cols-1", "md:grid-cols-2", "xl:grid-cols-4");
  });

  it("should not render error when error is null", () => {
    renderWithRouter(
      <PropertyList
        properties={mockProperties}
        isLoading={false}
        error={null}
      />
    );

    expect(
      screen.queryByText("Error Loading Properties")
    ).not.toBeInTheDocument();
  });

  it("should prioritize loading state over error state", () => {
    renderWithRouter(
      <PropertyList properties={[]} isLoading={true} error="Some error" />
    );

    const skeletons = screen
      .getAllByRole("generic")
      .filter((el) => el.className.includes("animate-pulse"));
    expect(skeletons.length).toBeGreaterThan(0);
    expect(
      screen.queryByText("Error Loading Properties")
    ).not.toBeInTheDocument();
  });

  it("should prioritize error state over empty state", () => {
    renderWithRouter(
      <PropertyList properties={[]} isLoading={false} error="Some error" />
    );

    expect(screen.getByText("Error Loading Properties")).toBeInTheDocument();
    expect(screen.queryByText("No Properties Found")).not.toBeInTheDocument();
  });
});
