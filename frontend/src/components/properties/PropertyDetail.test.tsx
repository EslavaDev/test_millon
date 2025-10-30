import { describe, it, expect } from "vitest";
import { render, screen } from "@testing-library/react";
import { BrowserRouter } from "react-router-dom";
import userEvent from "@testing-library/user-event";
import { PropertyDetail } from "./PropertyDetail";
import type { PropertyDetail as PropertyDetailType } from "../../lib/types/property.types";

const renderWithRouter = (ui: React.ReactElement) => {
  return render(<BrowserRouter>{ui}</BrowserRouter>);
};

const mockProperty: PropertyDetailType = {
  idProperty: "123",
  name: "Luxury Villa",
  address: "123 Main St, Beverly Hills, CA",
  price: 1250000,
  year: 2020,
  codeInternal: "LV-001",
  idOwner: "456",
  createdAt: "2024-01-01T00:00:00Z",
  updatedAt: "2024-01-01T00:00:00Z",
  owner: {
    idOwner: "456",
    name: "John Doe",
    address: "789 Owner St",
    photo: "https://example.com/owner.jpg",
    birthday: "1980-05-15T00:00:00Z",
  },
  images: [
    {
      idPropertyImage: "1",
      idProperty: "123",
      file: "https://example.com/image1.jpg",
      enabled: true,
    },
    {
      idPropertyImage: "2",
      idProperty: "123",
      file: "https://example.com/image2.jpg",
      enabled: true,
    },
  ],
  traces: [
    {
      idPropertyTrace: "1",
      idProperty: "123",
      dateSale: "2022-06-15T00:00:00Z",
      name: "Sale to John Doe",
      value: 1200000,
      tax: 24000,
    },
  ],
};

describe("PropertyDetail", () => {
  it("should render property name and price", () => {
    renderWithRouter(<PropertyDetail property={mockProperty} />);

    expect(screen.getByText("Luxury Villa")).toBeInTheDocument();
    expect(screen.getByText("$1,250,000")).toBeInTheDocument();
  });

  it("should render property information", () => {
    renderWithRouter(<PropertyDetail property={mockProperty} />);

    expect(screen.getByText("Year Built:")).toBeInTheDocument();
    expect(screen.getByText("2020")).toBeInTheDocument();
    expect(screen.getByText("Code Internal:")).toBeInTheDocument();
    expect(screen.getByText("LV-001")).toBeInTheDocument();
    expect(screen.getAllByText("Address:")).toHaveLength(2);
    expect(
      screen.getByText("123 Main St, Beverly Hills, CA")
    ).toBeInTheDocument();
  });

  it("should render owner information", () => {
    renderWithRouter(<PropertyDetail property={mockProperty} />);

    expect(screen.getByText("Owner Information")).toBeInTheDocument();
    expect(screen.getByText("John Doe")).toBeInTheDocument();
    expect(screen.getByText("789 Owner St")).toBeInTheDocument();
  });

  it("should render owner photo", () => {
    renderWithRouter(<PropertyDetail property={mockProperty} />);

    const ownerPhoto = screen.getByAltText("John Doe") as HTMLImageElement;
    expect(ownerPhoto).toBeInTheDocument();
    expect(ownerPhoto.src).toBe("https://example.com/owner.jpg");
  });

  it("should render main property image", () => {
    renderWithRouter(<PropertyDetail property={mockProperty} />);

    const mainImage = screen.getByAltText("Luxury Villa") as HTMLImageElement;
    expect(mainImage).toBeInTheDocument();
    expect(mainImage.src).toBe("https://example.com/image1.jpg");
  });

  it("should render image thumbnails when multiple images exist", () => {
    renderWithRouter(<PropertyDetail property={mockProperty} />);

    const thumbnails = screen
      .getAllByRole("button")
      .filter((button) => button.querySelector("img"));
    expect(thumbnails).toHaveLength(2);
  });

  it("should change main image when thumbnail is clicked", async () => {
    const user = userEvent.setup();
    renderWithRouter(<PropertyDetail property={mockProperty} />);

    const thumbnails = screen
      .getAllByRole("button")
      .filter((button) => button.querySelector("img"));

    await user.click(thumbnails[1]);

    const mainImage = screen.getByAltText("Luxury Villa") as HTMLImageElement;
    expect(mainImage.src).toBe("https://example.com/image2.jpg");
  });

  it("should render sale history when traces exist", () => {
    renderWithRouter(<PropertyDetail property={mockProperty} />);

    expect(screen.getByText("Sale History")).toBeInTheDocument();
    expect(screen.getByText("Sale to John Doe")).toBeInTheDocument();
    expect(screen.getByText("$1,200,000")).toBeInTheDocument();
    expect(screen.getByText("$24,000")).toBeInTheDocument();
  });

  it("should not render sale history when no traces exist", () => {
    const propertyWithoutTraces = { ...mockProperty, traces: [] };
    renderWithRouter(<PropertyDetail property={propertyWithoutTraces} />);

    expect(screen.queryByText("Sale History")).not.toBeInTheDocument();
  });

  it("should use placeholder image when no images exist", () => {
    const propertyWithoutImages = { ...mockProperty, images: [] };
    renderWithRouter(<PropertyDetail property={propertyWithoutImages} />);

    const mainImage = screen.getByAltText("Luxury Villa") as HTMLImageElement;
    expect(mainImage.src).toContain("placeholder-property.jpg");
  });

  it("should filter out disabled images", () => {
    const propertyWithDisabledImage: PropertyDetailType = {
      ...mockProperty,
      images: [
        {
          idPropertyImage: "1",
          idProperty: "123",
          file: "https://example.com/image1.jpg",
          enabled: true,
        },
        {
          idPropertyImage: "2",
          idProperty: "123",
          file: "https://example.com/image2.jpg",
          enabled: false,
        },
      ],
    };

    renderWithRouter(<PropertyDetail property={propertyWithDisabledImage} />);

    const mainImage = screen.getByAltText("Luxury Villa") as HTMLImageElement;
    expect(mainImage).toBeInTheDocument();

    const thumbnails = screen
      .queryAllByRole("button")
      .filter((button) => button.querySelector("img"));
    expect(thumbnails).toHaveLength(0);
  });

  it("should render back to properties button", () => {
    renderWithRouter(<PropertyDetail property={mockProperty} />);

    const backButton = screen.getByRole("link", {
      name: /back to properties/i,
    });
    expect(backButton).toBeInTheDocument();
    expect(backButton).toHaveAttribute("href", "/");
  });

  it("should format owner birthday", () => {
    renderWithRouter(<PropertyDetail property={mockProperty} />);

    expect(screen.getByText("Birthday:")).toBeInTheDocument();
    expect(screen.getByText(/1980/)).toBeInTheDocument();
  });

  it("should not render owner information section when owner is not provided", () => {
    const propertyWithoutOwner = { ...mockProperty, owner: null };
    renderWithRouter(<PropertyDetail property={propertyWithoutOwner} />);

    expect(screen.queryByText("Owner Information")).not.toBeInTheDocument();
  });
});
