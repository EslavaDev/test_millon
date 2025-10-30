import { describe, it, expect, vi } from "vitest";
import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { Button } from "./Button";

describe("Button", () => {
  it("should render button with children", () => {
    render(<Button>Click Me</Button>);
    expect(
      screen.getByRole("button", { name: "Click Me" })
    ).toBeInTheDocument();
  });

  it("should apply primary variant styles by default", () => {
    render(<Button>Primary</Button>);
    const button = screen.getByRole("button");
    expect(button).toHaveClass("bg-primary-600");
  });

  it("should apply secondary variant styles", () => {
    render(<Button variant="secondary">Secondary</Button>);
    const button = screen.getByRole("button");
    expect(button).toHaveClass("bg-secondary-600");
  });

  it("should apply outline variant styles", () => {
    render(<Button variant="outline">Outline</Button>);
    const button = screen.getByRole("button");
    expect(button).toHaveClass("border-2", "border-primary-600");
  });

  it("should apply ghost variant styles", () => {
    render(<Button variant="ghost">Ghost</Button>);
    const button = screen.getByRole("button");
    expect(button).toHaveClass("text-gray-700");
  });

  it("should apply small size styles", () => {
    render(<Button size="sm">Small</Button>);
    const button = screen.getByRole("button");
    expect(button).toHaveClass("px-3", "py-1.5", "text-sm");
  });

  it("should apply medium size styles by default", () => {
    render(<Button>Medium</Button>);
    const button = screen.getByRole("button");
    expect(button).toHaveClass("px-4", "py-2", "text-base");
  });

  it("should apply large size styles", () => {
    render(<Button size="lg">Large</Button>);
    const button = screen.getByRole("button");
    expect(button).toHaveClass("px-6", "py-3", "text-lg");
  });

  it("should be disabled when disabled prop is true", () => {
    render(<Button disabled>Disabled</Button>);
    const button = screen.getByRole("button");
    expect(button).toBeDisabled();
    expect(button).toHaveClass("disabled:opacity-50");
  });

  it("should call onClick handler when clicked", async () => {
    const user = userEvent.setup();
    const handleClick = vi.fn();
    render(<Button onClick={handleClick}>Click Me</Button>);

    await user.click(screen.getByRole("button"));
    expect(handleClick).toHaveBeenCalledTimes(1);
  });

  it("should not call onClick when disabled", async () => {
    const user = userEvent.setup();
    const handleClick = vi.fn();
    render(
      <Button onClick={handleClick} disabled>
        Disabled
      </Button>
    );

    await user.click(screen.getByRole("button"));
    expect(handleClick).not.toHaveBeenCalled();
  });

  it("should merge custom className with default styles", () => {
    render(<Button className="custom-class">Button</Button>);
    const button = screen.getByRole("button");
    expect(button).toHaveClass("custom-class");
    expect(button).toHaveClass("bg-primary-600");
  });

  it("should support type attribute", () => {
    render(<Button type="submit">Submit</Button>);
    const button = screen.getByRole("button");
    expect(button).toHaveAttribute("type", "submit");
  });
});
