import { describe, it, expect, vi } from "vitest";
import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { Select } from "./Select";

const mockOptions = [
  { value: "option1", label: "Option 1" },
  { value: "option2", label: "Option 2" },
  { value: "option3", label: "Option 3" },
];

describe("Select", () => {
  it("should render select with options", () => {
    render(<Select label="Sort By" options={mockOptions} />);
    expect(screen.getByLabelText("Sort By")).toBeInTheDocument();
    expect(
      screen.getByRole("option", { name: "Option 1" })
    ).toBeInTheDocument();
    expect(
      screen.getByRole("option", { name: "Option 2" })
    ).toBeInTheDocument();
    expect(
      screen.getByRole("option", { name: "Option 3" })
    ).toBeInTheDocument();
  });

  it("should render placeholder option when provided", () => {
    render(
      <Select
        label="Sort By"
        options={mockOptions}
        placeholder="Select an option"
      />
    );
    expect(
      screen.getByRole("option", { name: "Select an option" })
    ).toBeInTheDocument();
  });

  it("should make placeholder option disabled", () => {
    render(
      <Select
        label="Sort By"
        options={mockOptions}
        placeholder="Select an option"
      />
    );
    const placeholder = screen.getByRole("option", {
      name: "Select an option",
    }) as HTMLOptionElement;
    expect(placeholder).toHaveAttribute("disabled");
  });

  it("should associate label with select using htmlFor", () => {
    render(<Select label="Sort By" options={mockOptions} />);
    const label = screen.getByText("Sort By");
    const select = screen.getByLabelText("Sort By");
    expect(label).toHaveAttribute("for", "sort-by");
    expect(select).toHaveAttribute("id", "sort-by");
  });

  it("should display error message", () => {
    render(
      <Select
        label="Sort By"
        options={mockOptions}
        error="Selection is required"
      />
    );
    expect(screen.getByText("Selection is required")).toBeInTheDocument();
  });

  it("should apply error styles when error prop is provided", () => {
    render(
      <Select
        label="Sort By"
        options={mockOptions}
        error="Selection is required"
      />
    );
    const select = screen.getByLabelText("Sort By");
    expect(select).toHaveClass("border-red-500");
  });

  it("should apply error styles when hasError prop is true", () => {
    render(<Select label="Sort By" options={mockOptions} hasError />);
    const select = screen.getByLabelText("Sort By");
    expect(select).toHaveClass("border-red-500");
  });

  it("should set aria-invalid when error is present", () => {
    render(
      <Select
        label="Sort By"
        options={mockOptions}
        error="Selection is required"
      />
    );
    const select = screen.getByLabelText("Sort By");
    expect(select).toHaveAttribute("aria-invalid", "true");
  });

  it("should call onChange handler when value changes", async () => {
    const user = userEvent.setup();
    const handleChange = vi.fn();
    render(
      <Select label="Sort By" options={mockOptions} onChange={handleChange} />
    );

    const select = screen.getByLabelText("Sort By");
    await user.selectOptions(select, "option2");
    expect(handleChange).toHaveBeenCalled();
  });

  it("should be disabled when disabled prop is true", () => {
    render(<Select label="Sort By" options={mockOptions} disabled />);
    const select = screen.getByLabelText("Sort By");
    expect(select).toBeDisabled();
    expect(select).toHaveClass("disabled:bg-gray-100");
  });

  it("should support ref forwarding", () => {
    const ref = { current: null };
    render(<Select label="Sort By" options={mockOptions} ref={ref} />);
    expect(ref.current).toBeInstanceOf(HTMLSelectElement);
  });

  it("should merge custom className with default styles", () => {
    render(
      <Select label="Sort By" options={mockOptions} className="custom-class" />
    );
    const select = screen.getByLabelText("Sort By");
    expect(select).toHaveClass("custom-class");
    expect(select).toHaveClass("rounded-md");
  });
});
