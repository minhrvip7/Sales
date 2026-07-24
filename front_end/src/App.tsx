import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import MainLayout from './components/layout/MainLayout';
import Dashboard from './features/dashboard/Dashboard';
import ProductList from './features/product/ProductList';
import OrderList from './features/order/OrderList';
import CreateOrder from './features/order/CreateOrder';
import CategoryList from './features/category/CategoryList';
import UnitList from './features/unit/UnitList';
import InventoryList from './features/inventory/InventoryList';
import InventoryTransactionHistory from './features/inventory/InventoryTransactionHistory';
import GoodsReceiptList from './features/inventory/goods-receipt/GoodsReceiptList';
import GoodsReceiptForm from './features/inventory/goods-receipt/GoodsReceiptForm';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<MainLayout />}>
          <Route index element={<Dashboard />} />
          <Route path="products" element={<ProductList />} />
          <Route path="categories" element={<CategoryList />} />
          <Route path="units" element={<UnitList />} />
          <Route path="orders" element={<OrderList />} />
          <Route path="orders/new" element={<CreateOrder />} />
          <Route path="inventory" element={<InventoryList />} />
          <Route path="inventory/transactions/:productId" element={<InventoryTransactionHistory />} />
          <Route path="inventory/goods-receipt" element={<GoodsReceiptList />} />
          <Route path="inventory/goods-receipt/new" element={<GoodsReceiptForm />} />
          <Route path="inventory/goods-receipt/:id" element={<GoodsReceiptForm />} />
          <Route path="*" element={<Navigate to="/" replace />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
