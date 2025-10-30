export function formatCurrency(amount: number, currency: string = 'USD'): string {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency,
    minimumFractionDigits: 0,
    maximumFractionDigits: 0,
  }).format(amount);
}

export function formatDate(
  date: string | Date,
  format: 'short' | 'medium' | 'long' | 'full' = 'medium'
): string {
  const dateObj = typeof date === 'string' ? new Date(date) : date;

  const formatOptions: Record<string, Intl.DateTimeFormatOptions> = {
    short: { year: 'numeric', month: 'numeric', day: 'numeric' },
    medium: { year: 'numeric', month: 'short', day: 'numeric' },
    long: { year: 'numeric', month: 'long', day: 'numeric' },
    full: { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' },
  };

  return new Intl.DateTimeFormat('en-US', formatOptions[format]).format(dateObj);
}

export function getImageUrl(imagePath: string | null | undefined): string {
  if (!imagePath) {
    return '/placeholder-property.jpg';
  }

  if (imagePath.startsWith('http://') || imagePath.startsWith('https://')) {
    return imagePath;
  }

  const imagesBaseUrl = import.meta.env.VITE_IMAGES_URL || 'http://localhost:5000/images';
  return `${imagesBaseUrl}/${imagePath.replace(/^\/+/, '')}`;
}

export function truncateText(text: string, maxLength: number): string {
  if (text.length <= maxLength) {
    return text;
  }
  return `${text.substring(0, maxLength)}...`;
}

export function formatNumber(num: number): string {
  return new Intl.NumberFormat('en-US').format(num);
}
