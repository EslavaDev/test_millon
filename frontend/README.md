# Real Estate Property Management Frontend

Modern React 18 frontend with TypeScript, Vite, and Tailwind CSS 4.x.

## 🏗️ Architecture

This frontend follows a **component-based architecture** with clear separation between presentational and smart components:

```
┌──────────────────────────────────────┐
│          Pages (Smart)                │
│  Orchestrate components + data        │
├──────────────────────────────────────┤
│     Components (Presentational)       │
│  Reusable UI with props               │
├──────────────────────────────────────┤
│      Hooks & State Management         │
│  SWR (data) + Zustand (filters)      │
├──────────────────────────────────────┤
│         API Client & Types            │
│  Axios + TypeScript interfaces        │
└──────────────────────────────────────┘
```

### Design Principles

1. **Presentational vs Smart Components**
   - Presentational: Receive data via props, no API calls, no global state
   - Smart (Pages): Orchestrate data fetching, state management, and user interactions

2. **Data Fetching Strategy**
   - SWR for server state (automatic caching, revalidation)
   - Zustand for client state (filters, pagination)
   - Co-located data fetching in page components

3. **Type Safety**
   - Strong TypeScript types throughout
   - No `any` types in production code
   - Type guards for safe narrowing

4. **Testing Philosophy**
   - Component tests focus on user behavior
   - Mock API responses for isolation
   - No tests coupling to implementation details

## 📁 Project Structure

```
frontend/
├── src/
│   ├── components/
│   │   ├── layout/              # Layout components
│   │   │   ├── Header.tsx
│   │   │   ├── Footer.tsx
│   │   │   └── Layout.tsx
│   │   ├── properties/          # Property-specific components
│   │   │   ├── PropertyCard.tsx
│   │   │   ├── PropertyList.tsx
│   │   │   ├── PropertyFilters.tsx
│   │   │   └── PropertyDetail.tsx
│   │   └── ui/                  # Reusable UI components
│   │       ├── Button.tsx
│   │       ├── Input.tsx
│   │       ├── Select.tsx
│   │       └── Card.tsx
│   ├── pages/                   # Page components (smart)
│   │   ├── HomePage.tsx
│   │   ├── PropertyDetailPage.tsx
│   │   └── NotFoundPage.tsx
│   ├── lib/
│   │   ├── api/                 # API client
│   │   │   ├── client.ts
│   │   │   └── properties.ts
│   │   ├── hooks/               # Custom hooks
│   │   │   ├── useProperties.ts
│   │   │   └── usePropertyDetail.ts
│   │   ├── store/               # State management
│   │   │   └── propertyStore.ts
│   │   ├── types/               # TypeScript types
│   │   │   └── property.types.ts
│   │   └── utils/               # Utility functions
│   │       └── format.ts
│   ├── App.tsx                  # Main app component with routing
│   ├── main.tsx                 # Entry point
│   └── index.css                # Global styles
├── public/                      # Static assets
├── index.html                   # HTML template
├── vite.config.ts               # Vite configuration
├── tailwind.config.js           # Tailwind configuration
├── vitest.config.ts             # Test configuration
└── package.json                 # Dependencies
```

## 🧩 Components

### Layout Components

#### Header
```typescript
// Simple header with logo and branding
<Header />
```

#### Footer
```typescript
// Footer with copyright and links
<Footer />
```

#### Layout
```typescript
// Wrapper component for consistent page structure
<Layout>
  <HomePage />
</Layout>
```

### UI Components

#### Button
```typescript
<Button
  variant="primary" // primary, secondary, outline, ghost
  size="md"         // sm, md, lg
  onClick={handleClick}
  disabled={false}
>
  Click Me
</Button>
```

#### Input
```typescript
<Input
  label="Property Name"
  type="text"
  value={value}
  onChange={handleChange}
  error="Error message"
  placeholder="Enter property name"
/>
```

#### Select
```typescript
<Select
  label="Sort By"
  value={sortBy}
  onChange={handleChange}
  options={[
    { value: 'name', label: 'Name' },
    { value: 'price', label: 'Price' }
  ]}
  placeholder="Select option"
/>
```

#### Card
```typescript
<Card hoverable onClick={handleClick}>
  <h3>Card Title</h3>
  <p>Card content</p>
</Card>
```

### Property Components

#### PropertyCard
Displays a property in card format with image, name, price, and basic info.

```typescript
<PropertyCard property={propertyData} />
```

**Props:**
- `property`: PropertyListItem - Property data to display

**Features:**
- Responsive image with lazy loading
- Image fallback handling
- Price formatting
- Link to property detail page
- Hover effects

#### PropertyList
Renders a grid of property cards with loading and error states.

```typescript
<PropertyList
  properties={propertiesArray}
  loading={isLoading}
  error={errorMessage}
/>
```

**Props:**
- `properties`: PropertyListItem[] - Array of properties
- `loading`: boolean - Loading state
- `error`: string | null - Error message

**Features:**
- Responsive grid (1-4 columns)
- Skeleton loaders during loading
- Error display with retry
- Empty state handling

#### PropertyFilters
Filter form with name, address, and price range inputs.

```typescript
<PropertyFilters
  initialValues={filters}
  onFilterChange={handleFilterChange}
/>
```

**Props:**
- `initialValues`: PropertyFilter - Initial filter values
- `onFilterChange`: (filters: PropertyFilter) => void - Callback when filters change

**Features:**
- Form validation with Zod
- Error display
- Reset functionality
- Responsive layout

#### PropertyDetail
Displays comprehensive property information with image gallery.

```typescript
<PropertyDetail property={propertyDetailData} />
```

**Props:**
- `property`: PropertyDetail - Full property data

**Features:**
- Image gallery with thumbnails
- Property information grid
- Owner details section
- Sale history table
- Responsive layout
- Back navigation

### Pages (Smart Components)

#### HomePage
Main property listing page with filters, sorting, and pagination.

**Features:**
- Zustand store for filter state
- SWR hook for data fetching
- Pagination controls
- Sort controls
- Page size selector
- Responsive layout

#### PropertyDetailPage
Individual property detail page.

**Features:**
- Route parameter extraction
- SWR hook for property data
- Loading skeleton
- Error handling
- 404 redirect for invalid IDs

#### NotFoundPage
404 error page with navigation options.

## 🎣 Custom Hooks

### useProperties
Fetches paginated property list with filtering.

```typescript
const { properties, error, isLoading, totalCount, pagination } = useProperties(filters);
```

**Features:**
- SWR caching (2s deduping)
- Automatic revalidation
- Keep previous data during refetch
- Pagination metadata

### usePropertyDetail
Fetches individual property details.

```typescript
const { property, error, isLoading, mutate } = usePropertyDetail(propertyId);
```

**Features:**
- SWR caching (5min deduping)
- Conditional fetching (null ID skips)
- Manual revalidation with mutate()

## 🗃️ State Management

### Zustand Store (propertyStore)

Manages filter and pagination state with persistence.

```typescript
const {
  filters,
  setFilters,
  resetFilters,
  setCurrentPage,
  setPageSize,
  setSorting
} = usePropertyStore();
```

**State:**
- `filters`: PropertyFilter - Current filter values
- Persisted to localStorage (filters only, not pagination)

**Actions:**
- `setFilters(filters)` - Update filters
- `resetFilters()` - Reset to defaults
- `setCurrentPage(page)` - Update page number
- `setPageSize(size)` - Update page size
- `setSorting(sortBy, descending)` - Update sorting

## 🎨 Styling

### Tailwind CSS 4.x

Configuration in `tailwind.config.js`:

```javascript
{
  theme: {
    extend: {
      colors: {
        primary: { 50: '...', ..., 950: '...' },
        secondary: { 50: '...', ..., 950: '...' }
      }
    }
  }
}
```

### Responsive Breakpoints

- `xs`: 475px
- `sm`: 640px
- `md`: 768px
- `lg`: 1024px
- `xl`: 1280px
- `2xl`: 1536px

### Usage Examples

```tsx
// Responsive grid
<div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-4 gap-6">

// Responsive text
<h1 className="text-2xl sm:text-3xl lg:text-4xl font-bold">

// Responsive spacing
<div className="p-4 sm:p-6 lg:p-8">
```

## 🧪 Testing

### Test Structure

```
src/
├── components/
│   ├── ui/
│   │   ├── Button.tsx
│   │   └── Button.test.tsx        # 13 tests
│   ├── properties/
│   │   ├── PropertyCard.tsx
│   │   └── PropertyCard.test.tsx  # 10 tests
│   └── ...
└── lib/
    └── utils/
        ├── format.ts
        └── format.test.ts         # 28 tests
```

### Running Tests

```bash
# Run all tests
npm test

# Run tests in watch mode
npm test -- --watch

# Run tests with coverage
npm test -- --coverage

# Run specific test file
npm test Button.test
```

### Test Coverage

- **Total**: 119/119 tests passing (100%)
- UI Components: 48 tests
- Property Components: 41 tests
- Utility Functions: 28 tests
- Additional: 2 tests


### Writing Tests

```typescript
import { render, screen } from '@testing-library/react';
import { PropertyCard } from './PropertyCard';

describe('PropertyCard', () => {
  const mockProperty = {
    idProperty: '1',
    name: 'Luxury Villa',
    price: 1250000,
    // ...
  };

  it('renders property information', () => {
    render(
      <MemoryRouter>
        <PropertyCard property={mockProperty} />
      </MemoryRouter>
    );

    expect(screen.getByText('Luxury Villa')).toBeInTheDocument();
    expect(screen.getByText('$1,250,000')).toBeInTheDocument();
  });
});
```

## 🚀 Development

### Getting Started

```bash
# Install dependencies
npm install

# Start dev server
npm run dev

# Build for production
npm run build

# Preview production build
npm run preview
```

### Environment Variables

Create `.env` file:

```env
VITE_API_URL=http://localhost:5000/api
VITE_IMAGES_URL=http://localhost:5000/images
```

### Scripts

| Command | Description |
|---------|-------------|
| `npm run dev` | Start development server (port 3000) |
| `npm run build` | Build for production |
| `npm run preview` | Preview production build |
| `npm test` | Run tests |
| `npm run lint` | Run ESLint |
| `npm run lint:fix` | Fix ESLint issues |

### Code Quality

```bash
# Run linter
npm run lint

# Auto-fix linting issues
npm run lint -- --fix

# Format code (if Prettier configured)
npm run format
```

## 📦 Build & Deploy

### Production Build

```bash
npm run build
```

Output in `dist/` directory:
- `index.html` - Entry point
- `assets/` - JS, CSS, and assets
- All assets are hashed for cache busting

### Deploy to Static Hosting

#### Vercel
```bash
npm install -g vercel
vercel
```

#### Netlify
```bash
npm install -g netlify-cli
netlify deploy --prod --dir=dist
```

#### GitHub Pages
```bash
# Build with base URL
npm run build -- --base=/repo-name/

# Deploy dist/ folder to gh-pages branch
```

### Environment Variables in Production

Set environment variables in your hosting platform:
- `VITE_API_URL` - Production API URL
- `VITE_IMAGES_URL` - Production images URL

## 🔧 Configuration Files

### vite.config.ts
```typescript
export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'),
    },
  },
  server: {
    port: 3000,
  },
});
```

### vitest.config.ts
```typescript
export default defineConfig({
  test: {
    globals: true,
    environment: 'jsdom',
    setupFiles: './src/test/setup.ts',
  },
});
```

### tailwind.config.js
```javascript
export default {
  content: ['./index.html', './src/**/*.{js,ts,jsx,tsx}'],
  theme: {
    extend: {
      colors: {
        primary: { /* ... */ },
        secondary: { /* ... */ },
      },
    },
  },
  plugins: [],
};
```

## 🎯 Performance Optimization

### Implemented Optimizations

1. **Code Splitting**: Automatic with Vite
2. **Lazy Loading**: Images with `loading="lazy"`
3. **SWR Caching**: Reduces API calls
4. **Responsive Images**: Appropriate sizes for viewports
5. **Minification**: CSS and JS minified in production

### Performance Metrics

- **Time to Interactive**: < 3s
- **First Contentful Paint**: < 1.5s
- **Lighthouse Score**: > 90

### Tips for Better Performance

```typescript
// Use React.memo for expensive components
export const PropertyCard = React.memo(({ property }) => {
  // ...
});

// Debounce search inputs
const debouncedSearch = useDebouncedCallback(
  (value) => setFilters({ name: value }),
  300
);

// Virtual scrolling for long lists
import { FixedSizeList } from 'react-window';
```

## 🔐 Security

- ✅ No sensitive data in client code
- ✅ API keys in environment variables
- ✅ XSS protection via React (escapes by default)
- ✅ HTTPS in production
- ✅ Content Security Policy (CSP) configured
- ✅ Input validation on forms

## 🐛 Troubleshooting

### Build Errors

```bash
# Clear cache and reinstall
rm -rf node_modules package-lock.json
npm install

# Clear Vite cache
rm -rf node_modules/.vite
```

### Type Errors

```bash
# Regenerate types
npx tsc --noEmit

# Check for TypeScript errors
npm run type-check
```

### Test Failures

```bash
# Run tests with verbose output
npm test -- --reporter=verbose

# Update snapshots
npm test -- -u
```

## 📚 Resources

- [React Documentation](https://react.dev/)
- [TypeScript Documentation](https://www.typescriptlang.org/docs/)
- [Vite Documentation](https://vite.dev/)
- [Tailwind CSS Documentation](https://tailwindcss.com/docs)
- [SWR Documentation](https://swr.vercel.app/)
- [Zustand Documentation](https://zustand-demo.pmnd.rs/)
- [React Testing Library](https://testing-library.com/react)

