import { describe, it, expect, vi } from "vitest";
import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { Card } from "./Card";

describe("Card", () => {
  it("should render card with children", () => {
    render(<Card>Card Content</Card>);
    expect(screen.getByText("Card Content")).toBeInTheDocument();
  });

  it("should apply base styles", () => {
    const { container } = render(<Card>Content</Card>);
    const card = container.firstChild as HTMLElement;
    expect(card).toHaveClass("bg-white", "rounded-lg", "shadow-md");
  });

  it("should apply hover styles when hoverable is true", () => {
    const { container } = render(<Card hoverable>Content</Card>);
    const card = container.firstChild as HTMLElement;
    expect(card).toHaveClass("hover:shadow-lg", "cursor-pointer");
  });

  it("should not apply hover styles when hoverable is false", () => {
    render(<Card hoverable={false}>Content</Card>);
    const card = screen.getByText("Content").parentElement;
    expect(card).not.toHaveClass("hover:shadow-lg");
  });

  it("should call onClick handler when clicked", async () => {
    const user = userEvent.setup();
    const handleClick = vi.fn();
    render(<Card onClick={handleClick}>Content</Card>);

    const card = screen.getByRole("button");
    await user.click(card);
    expect(handleClick).toHaveBeenCalledTimes(1);
  });

  it('should have role="button" when onClick is provided', () => {
    const handleClick = vi.fn();
    render(<Card onClick={handleClick}>Content</Card>);
    const card = screen.getByRole("button");
    expect(card).toHaveAttribute("role", "button");
  });

  it("should be keyboard accessible when onClick is provided", async () => {
    const user = userEvent.setup();
    const handleClick = vi.fn();
    render(<Card onClick={handleClick}>Content</Card>);

    const card = screen.getByRole("button");
    card.focus();
    await user.keyboard("{Enter}");
    expect(handleClick).toHaveBeenCalledTimes(1);
  });

  it("should be keyboard accessible with Space key", async () => {
    const user = userEvent.setup();
    const handleClick = vi.fn();
    render(<Card onClick={handleClick}>Content</Card>);

    const card = screen.getByRole("button");
    card.focus();
    await user.keyboard(" ");
    expect(handleClick).toHaveBeenCalledTimes(1);
  });

  it("should have tabIndex when onClick is provided", () => {
    const handleClick = vi.fn();
    render(<Card onClick={handleClick}>Content</Card>);
    const card = screen.getByRole("button");
    expect(card).toHaveAttribute("tabIndex", "0");
  });

  it("should merge custom className with default styles", () => {
    const { container } = render(<Card className="custom-class">Content</Card>);
    const card = container.firstChild as HTMLElement;
    expect(card).toHaveClass("custom-class");
    expect(card).toHaveClass("bg-white");
  });
});
