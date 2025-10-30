import { describe, it, expect } from "vitest";
import {
  formatCurrency,
  formatDate,
  getImageUrl,
  truncateText,
  formatNumber,
} from "./format";

describe("formatCurrency", () => {
  it("should format a positive number as USD currency", () => {
    expect(formatCurrency(1234567.89)).toBe("$1,234,568");
  });

  it("should format zero correctly", () => {
    expect(formatCurrency(0)).toBe("$0");
  });

  it("should format negative numbers", () => {
    expect(formatCurrency(-1000)).toBe("-$1,000");
  });

  it("should handle small numbers", () => {
    expect(formatCurrency(5)).toBe("$5");
  });

  it("should handle large numbers", () => {
    expect(formatCurrency(1000000000)).toBe("$1,000,000,000");
  });

  it("should support different currencies", () => {
    const result = formatCurrency(100, "EUR");
    expect(result).toContain("100");
  });
});

describe("formatDate", () => {
  it("should format a date string in medium format", () => {
    const date = new Date(2023, 5, 15);
    const result = formatDate(date);
    expect(result).toContain("2023");
    expect(result).toContain("Jun");
  });

  it("should format a Date object", () => {
    const date = new Date("2023-06-15");
    const result = formatDate(date);
    expect(result).toContain("2023");
    expect(result).toContain("Jun");
  });

  it("should format in short format", () => {
    const date = new Date(2023, 5, 15);
    const result = formatDate(date, "short");
    expect(result).toContain("6");
    expect(result).toContain("2023");
  });

  it("should format in long format", () => {
    const date = new Date(2023, 5, 15);
    const result = formatDate(date, "long");
    expect(result).toContain("June");
    expect(result).toContain("2023");
  });

  it("should format in full format", () => {
    const date = new Date(2023, 5, 15);
    const result = formatDate(date, "full");
    expect(result).toContain("June");
    expect(result).toContain("2023");
  });
});

describe("getImageUrl", () => {
  it("should return placeholder for null", () => {
    expect(getImageUrl(null)).toBe("/placeholder-property.jpg");
  });

  it("should return placeholder for undefined", () => {
    expect(getImageUrl(undefined)).toBe("/placeholder-property.jpg");
  });

  it("should return placeholder for empty string", () => {
    expect(getImageUrl("")).toBe("/placeholder-property.jpg");
  });

  it("should return full URL if already a full HTTP URL", () => {
    const url = "http://example.com/image.jpg";
    expect(getImageUrl(url)).toBe(url);
  });

  it("should return full URL if already a full HTTPS URL", () => {
    const url = "https://example.com/image.jpg";
    expect(getImageUrl(url)).toBe(url);
  });

  it("should construct full URL for relative path", () => {
    const result = getImageUrl("images/property1.jpg");
    expect(result).toContain("images/property1.jpg");
    expect(result).toMatch(/^http/);
  });

  it("should handle paths with leading slash", () => {
    const result = getImageUrl("/images/property1.jpg");
    expect(result).toContain("images/property1.jpg");
    expect(result).not.toContain("//images");
  });
});

describe("truncateText", () => {
  it("should not truncate text shorter than max length", () => {
    expect(truncateText("Hello", 10)).toBe("Hello");
  });

  it("should not truncate text equal to max length", () => {
    expect(truncateText("Hello", 5)).toBe("Hello");
  });

  it("should truncate text longer than max length", () => {
    expect(truncateText("Hello World", 5)).toBe("Hello...");
  });

  it("should truncate long text correctly", () => {
    const longText =
      "This is a very long property description that needs to be truncated";
    const result = truncateText(longText, 20);
    expect(result).toBe("This is a very long ...");
    expect(result.length).toBe(23);
  });

  it("should handle empty string", () => {
    expect(truncateText("", 10)).toBe("");
  });
});

describe("formatNumber", () => {
  it("should format numbers with thousand separators", () => {
    expect(formatNumber(1234567)).toBe("1,234,567");
  });

  it("should format small numbers without separators", () => {
    expect(formatNumber(123)).toBe("123");
  });

  it("should handle zero", () => {
    expect(formatNumber(0)).toBe("0");
  });

  it("should handle negative numbers", () => {
    expect(formatNumber(-12345)).toBe("-12,345");
  });

  it("should preserve decimals", () => {
    const result = formatNumber(1234.56);
    expect(result).toContain("1,234");
  });
});
