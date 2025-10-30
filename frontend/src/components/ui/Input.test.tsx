import { describe, it, expect, vi } from "vitest";
import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { Input } from "./Input";

describe("Input", () => {
  it("should render input without label", () => {
    render(<Input placeholder="Enter text" />);
    expect(screen.getByPlaceholderText("Enter text")).toBeInTheDocument();
  });

  it("should render input with label", () => {
    render(<Input label="Name" />);
    expect(screen.getByLabelText("Name")).toBeInTheDocument();
  });

  it("should associate label with input using htmlFor", () => {
    render(<Input label="Email" />);
    const label = screen.getByText("Email");
    const input = screen.getByLabelText("Email");
    expect(label).toHaveAttribute("for", "email");
    expect(input).toHaveAttribute("id", "email");
  });

  it("should display error message", () => {
    render(<Input label="Name" error="Name is required" />);
    expect(screen.getByText("Name is required")).toBeInTheDocument();
  });

  it("should apply error styles when error prop is provided", () => {
    render(<Input label="Name" error="Name is required" />);
    const input = screen.getByLabelText("Name");
    expect(input).toHaveClass("border-red-500");
  });

  it("should apply error styles when hasError prop is true", () => {
    render(<Input label="Name" hasError />);
    const input = screen.getByLabelText("Name");
    expect(input).toHaveClass("border-red-500");
  });

  it("should set aria-invalid when error is present", () => {
    render(<Input label="Name" error="Name is required" />);
    const input = screen.getByLabelText("Name");
    expect(input).toHaveAttribute("aria-invalid", "true");
  });

  it("should set aria-describedby when error is present", () => {
    render(<Input label="Name" error="Name is required" />);
    const input = screen.getByLabelText("Name");
    expect(input).toHaveAttribute("aria-describedby", "name-error");
  });

  it("should call onChange handler when value changes", async () => {
    const user = userEvent.setup();
    const handleChange = vi.fn();
    render(<Input label="Name" onChange={handleChange} />);

    const input = screen.getByLabelText("Name");
    await user.type(input, "John");
    expect(handleChange).toHaveBeenCalled();
  });

  it("should be disabled when disabled prop is true", () => {
    render(<Input label="Name" disabled />);
    const input = screen.getByLabelText("Name");
    expect(input).toBeDisabled();
    expect(input).toHaveClass("disabled:bg-gray-100");
  });

  it("should support different input types", () => {
    render(<Input label="Email" type="email" />);
    const input = screen.getByLabelText("Email");
    expect(input).toHaveAttribute("type", "email");
  });

  it("should support ref forwarding", () => {
    const ref = { current: null };
    render(<Input label="Name" ref={ref} />);
    expect(ref.current).toBeInstanceOf(HTMLInputElement);
  });

  it("should merge custom className with default styles", () => {
    render(<Input label="Name" className="custom-class" />);
    const input = screen.getByLabelText("Name");
    expect(input).toHaveClass("custom-class");
    expect(input).toHaveClass("rounded-md"); // Still has default styles
  });
});
