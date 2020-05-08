//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Windows;
//using System.Windows.Input;

//namespace WpfClient.ViewModels
//{
//    public class MainViewModel : ViewModelBase
//    {
//        public MainViewModel()
//        {
//            unitOfWork = new UnitOfWork();
//        }

//        private UnitOfWork unitOfWork;


//        #region command
//        private Command CreateCommand_imp;
//        private Command UpdateCommand_imp;
//        private Command DeleteCommand_imp;
//        private Command AddCommand_imp;
//        private Command RemoveCommand_imp;


//        public ICommand CreateCommand
//        {
//            get
//            {
//                if (CreateCommand_imp == null)
//                {
//                    CreateCommand_imp = new Command(CreateNewProduct);
//                }
//                return CreateCommand_imp;
//            }
//        }
//        public ICommand UpdateCommand
//        {
//            get
//            {
//                if (UpdateCommand_imp == null)
//                {
//                    UpdateCommand_imp = new Command(UpdateProducts);
//                }
//                return UpdateCommand_imp;
//            }
//        }
//        public ICommand DeleteCommand
//        {
//            get
//            {
//                if (DeleteCommand_imp == null)
//                {
//                    DeleteCommand_imp = new Command(DeleteProduct);
//                }
//                return DeleteCommand_imp;
//            }
//        }
//        public ICommand AddCommand
//        {
//            get
//            {
//                if (AddCommand_imp == null)
//                {
//                    AddCommand_imp = new Command(AddProductToCart);
//                }
//                return AddCommand_imp;
//            }
//        }
//        public ICommand RemoveCommand
//        {
//            get
//            {
//                if (RemoveCommand_imp == null)
//                {
//                    RemoveCommand_imp = new Command(RemoveProductFromCart);
//                }
//                return RemoveCommand_imp;
//            }
//        }



//        public void CreateNewProduct()
//        {
//            ///TODO       same names condition 
//            string ProdName;
//            double ProdCost;
//            int ProdQuan;
//            if ((ProdName = ShopProductName) != null && double.TryParse(ShopProductCost, out ProdCost) && int.TryParse(ShopProductQuan, out ProdQuan))
//            {
//                Shop shop = unitOfWork.Shops.Get(1);
//                if (shop.products.Exists(x => x.Name == ProdName))
//                {
//                    MessageBox.Show($"We already have {ProdName}", "Create Error", MessageBoxButton.OK);
//                    return;
//                }
//                try
//                {
//                    shop.products.Add(new Stock(new Product(ProdName, ProdCost), ProdQuan));
//                    unitOfWork.Shops.Update(shop);
//                    unitOfWork.Save();
//                    _isCreate = false;
//                    NotifyPropertyChanged("IsCreate");
//                    NotifyPropertyChanged("IsCreate_invert");
//                    NotifyPropertyChanged("ShopStocksObs");
//                }
//                catch (Exception e)
//                {
//                    MessageBox.Show(e.Message, "Add to database Error", MessageBoxButton.OK);
//                }
//            }
//            else
//            {
//                MessageBox.Show("Create new product ERROR.\nInvalid input data!", "Create Error", MessageBoxButton.OK);
//            }
//        }
//        public void UpdateProducts()
//        {
//            MessageBoxResult ok = MessageBox.Show("All information will be update", "Update database", MessageBoxButton.OKCancel);
//            if (ok == MessageBoxResult.OK)
//            {
//                try
//                {
//                    unitOfWork.Save();
//                    NotifyPropertyChanged("ShopStocksObs");
//                }
//                catch (Exception e)
//                {
//                    MessageBox.Show(e.Message, "Update database Error", MessageBoxButton.OK);
//                }
//            }
//        }
//        public void DeleteProduct()
//        {
//            if (_selectedProductShop != null)
//            {
//                MessageBoxResult ok = MessageBox.Show($"Do you want delete {_selectedProductShop.Name}", "Delete product", MessageBoxButton.OKCancel);
//                if (ok == MessageBoxResult.OK)
//                {
//                    try
//                    {
//                        Shop shop = unitOfWork.Shops.Get(1);
//                        Cart cart = unitOfWork.Carts.Get(1);
//                        shop.products.RemoveAll(x =>
//                        x.Name == _selectedProductShop.Name &&
//                        x.cost == _selectedProductShop.cost
//                        );
//                        cart.products.RemoveAll(x =>
//                        x.Name == _selectedProductShop.Name
//                        );
//                        unitOfWork.Shops.Update(shop);
//                        unitOfWork.Carts.Update(cart);
//                        unitOfWork.Save();
//                        NotifyPropertyChanged("ShopStocksObs");
//                        NotifyPropertyChanged("CartStocksObs");
//                    }
//                    catch (Exception e)
//                    {
//                        MessageBox.Show(e.Message, "Delete product Error", MessageBoxButton.OK);
//                    }
//                }
//            }
//            else
//            {
//                MessageBox.Show("Please choose the product", "Delete product Error", MessageBoxButton.OK);
//            }
//        }
//        public void AddProductToCart()
//        {
//            if (_selectedProductShop != null)
//            {
//                if (_selectedProductShop.quantity < 1)
//                {
//                    MessageBox.Show("We don't have this product", "Add product to cart Error", MessageBoxButton.OK);
//                    return;
//                }
//                try
//                {
//                    Shop shop = unitOfWork.Shops.Get(1);
//                    Cart cart = unitOfWork.Carts.Get(1);
//                    if (cart.products == null)
//                        cart.products = new List<Stock>();
//                    if (!cart.products.Exists(x => x.Name == _selectedProductShop.Name))
//                    {
//                        Stock ShopStock = shop.products.Find(x => x.Name == _selectedProductShop.Name);
//                        cart.products.Add(new Stock(new Product(ShopStock.Name,ShopStock.cost),0));
//                    }
//                    cart.products.Find(x => x.Name == _selectedProductShop.Name).quantity += 1;
//                    shop.products.Find(x => x.Name == _selectedProductShop.Name).quantity -= 1;
//                    unitOfWork.Shops.Update(shop);
//                    unitOfWork.Carts.Update(cart);
//                    unitOfWork.Save();
//                    NotifyPropertyChanged("ShopStocksObs");
//                    NotifyPropertyChanged("CartStocksObs");
//                    NotifyPropertyChanged("SUM");
//                }
//                catch (Exception e)
//                {
//                    MessageBox.Show(e.Message, "Add product to cart Error", MessageBoxButton.OK);
//                }
//            }
//            else
//            {
//                MessageBox.Show("Please choose the product", "Add product to cart Error", MessageBoxButton.OK);
//            }
//        }
//        public void RemoveProductFromCart()
//        {
//            if (_selectedProductCart != null)
//            {
//                try
//                {
//                    Shop shop = unitOfWork.Shops.Get(1);
//                    Cart cart = unitOfWork.Carts.Get(1);

//                    cart.products.Find(x => x.Name == _selectedProductCart.Name).quantity -= 1;
//                    shop.products.Find(x => x.Name == _selectedProductCart.Name).quantity += 1;

//                    if (cart.products.Find(x => x.Name == _selectedProductCart.Name).quantity == 0)
//                    {
//                        cart.products.RemoveAll(x => x.Name == _selectedProductCart.Name);
//                        _selectedProductCart = null;
//                    }
//                    unitOfWork.Shops.Update(shop);
//                    unitOfWork.Carts.Update(cart);
//                    unitOfWork.Save();
//                    NotifyPropertyChanged("ShopStocksObs");
//                    NotifyPropertyChanged("CartStocksObs");
//                    NotifyPropertyChanged("SUM");
//                }
//                catch (Exception e)
//                {
//                    MessageBox.Show(e.Message, "Remove product from cart Error", MessageBoxButton.OK);
//                }
//            }
//            else
//            {
//                MessageBox.Show("Please choose the product", "Remove product from cart Error", MessageBoxButton.OK);
//            }
//        }
//        #endregion command


//        #region ShopStock
//        public ObservableCollection<Stock> ShopStocksObs
//        {
//            get
//            {
//                return new ObservableCollection<Stock>(ShopStocks);
//            }
//        }
//        private List<Stock> _shopStocks;
//        public List<Stock> ShopStocks
//        {
//            get
//            {
//                _shopStocks = unitOfWork.Shops.Get(1).products;
//                return _shopStocks;
//            }
//            set
//            {
//                _shopStocks = value;
//            }
//        }

//        private Stock _selectedProductShop;
//        public Stock SelectedProductShop
//        {
//            get { return _selectedProductShop; }
//            set
//            {
//                if (value != null)
//                    _selectedProductShop = value;
//                NotifyPropertyChanged("ShopProductName");
//                NotifyPropertyChanged("ShopProductCost");
//                NotifyPropertyChanged("ShopProductQuan");
//            }
//        }

//        private string _shopProductName;
//        public string ShopProductName
//        {
//            get
//            {
//                if (!_isCreate)
//                {
//                    if (_selectedProductShop != null)
//                    {
//                        _shopProductName = _selectedProductShop.Name;
//                    }
//                    else
//                    {
//                        _shopProductName = "Name";
//                    }
//                    return _shopProductName;
//                }
//                else
//                {
//                    return CreateStockName;
//                }
//            }
//            set
//            {
//                if (!_isCreate)
//                {
//                    if (value != "" && _selectedProductShop != null)
//                    {
//                        _selectedProductShop.Name = value;
//                        unitOfWork.Stocks.Update(_selectedProductShop);
//                        NotifyPropertyChanged("ShopStocksObs");
//                    }
//                    NotifyPropertyChanged();
//                }
//                else
//                {
//                    CreateStockName = value;
//                    NotifyPropertyChanged();

//                }
//            }
//        }

//        private string _shopProductCost;
//        public string ShopProductCost
//        {
//            get
//            {
//                if (!_isCreate)
//                {
//                    if (_selectedProductShop != null && _shopProductCost != "")
//                    {
//                        _shopProductCost = _selectedProductShop.cost.ToString();
//                    }
//                    else
//                    {
//                        _shopProductCost = "Cost";
//                    }
//                    return _shopProductCost;
//                }
//                else
//                {
//                    return CreateStockCost.ToString();
//                }
//            }
//            set
//            {
//                if (!_isCreate)
//                {
//                    if (value != "" && _selectedProductShop != null)
//                    {
//                        _selectedProductShop.cost = double.Parse(value);
//                        unitOfWork.Stocks.Update(_selectedProductShop);
//                        NotifyPropertyChanged("ShopStocksObs");
//                    }
//                    NotifyPropertyChanged();
//                }
//                else
//                {
//                    CreateStockCost = value;
//                    NotifyPropertyChanged();
//                }
//            }
//        }

//        private string _shopProductQuan;
//        public string ShopProductQuan
//        {
//            get
//            {
//                if (!_isCreate)
//                {
//                    if (_selectedProductShop != null && _shopProductQuan != "")
//                    {
//                        _shopProductQuan = _selectedProductShop.quantity.ToString();
//                    }
//                    else
//                    {
//                        _shopProductQuan = "Quantity";
//                    }
//                    return _shopProductQuan;
//                }
//                else
//                {
//                    return CreateStockQuan;
//                }
//            }
//            set
//            {
//                if (!_isCreate)
//                {
//                    if (value != "" && _selectedProductShop != null)
//                    {
//                        _selectedProductShop.quantity = int.Parse(value);
//                        unitOfWork.Stocks.Update(_selectedProductShop);
//                        NotifyPropertyChanged("ShopStocksObs");
//                    }
//                    NotifyPropertyChanged();
//                }
//                else
//                {
//                    CreateStockQuan = value;
//                    NotifyPropertyChanged();
//                }
//            }
//        }
//        #endregion ShopStock


//        #region CartStock
//        public ObservableCollection<Stock> CartStocksObs
//        {
//            get
//            {
//                return new ObservableCollection<Stock>(CartStocks);
//            }
//        }
//        private List<Stock> _cartStocks;
//        public List<Stock> CartStocks
//        {
//            get
//            {
//                _cartStocks = unitOfWork.Carts.Get(1).products;
//                return _cartStocks;
//            }
//            set
//            {
//                _cartStocks = value;
//            }
//        }

//        private Stock _selectedProductCart;
//        public Stock SelectedProductCart
//        {
//            get { return _selectedProductCart; }
//            set
//            {
//                if (value != null)
//                    _selectedProductCart = value;
//                NotifyPropertyChanged("CartProductName");
//                NotifyPropertyChanged("CartProductCost");
//                NotifyPropertyChanged("CartProductQuan");
//            }
//        }

//        private string _cartProductName;
//        public string CartProductName
//        {
//            get
//            {
//                if (_selectedProductCart != null)
//                {
//                    _cartProductName = _selectedProductCart.Name;
//                }
//                else
//                {
//                    _cartProductName = "Name";
//                }
//                return _cartProductName;
//            }
//        }

//        private string _cartProductCost;
//        public string CartProductCost
//        {
//            get
//            {
//                if (_selectedProductCart != null)
//                {
//                    _cartProductCost = _selectedProductCart.cost.ToString();
//                }
//                else
//                {
//                    _cartProductCost = "Cost";
//                }
//                return _cartProductCost;
//            }
//        }

//        private string _cartProductQuan;
//        public string CartProductQuan
//        {
//            get
//            {
//                if (_selectedProductCart != null)
//                {
//                    _cartProductQuan = _selectedProductCart.quantity.ToString();
//                }
//                else
//                {
//                    _cartProductQuan = "Quantity";
//                }
//                return _cartProductQuan;
//            }
//        }
//        #endregion CartStock


//        #region CreateButton
//        private bool _isCreate = false;
//        public bool IsCreate
//        {
//            get
//            {
//                return _isCreate;
//            }
//            set
//            {
//                _isCreate = value;
//                if (_isCreate)
//                {
//                    _selectedProductShop = null;
//                    CreateStockName =
//                        CreateStockCost =
//                        CreateStockQuan = "";
//                    NotifyPropertyChanged("ShopProductName");
//                    NotifyPropertyChanged("ShopProductCost");
//                    NotifyPropertyChanged("ShopProductQuan");
//                }
//                NotifyPropertyChanged();
//                NotifyPropertyChanged("IsCreate_invert");
//            }
//        }
//        public bool IsCreate_invert
//        {
//            get
//            {
//                return !_isCreate;
//            }
//        }

//        private string CreateStockName;
//        private string CreateStockCost;
//        private string CreateStockQuan;
//        #endregion CreateButton


//        #region Other
//        public string SUM
//        {
//            get
//            {
//                double returned = 0;
//                if (CartStocks != null)
//                {
//                    foreach (Stock stock in _cartStocks)
//                    {
//                        returned += stock.cost * stock.quantity;
//                    }
//                }
//                return returned.ToString();
//            }
//        }

//        public string ShopName
//        {
//            get { return unitOfWork.Shops.Get(1).Name; }
//            set { NotifyPropertyChanged(); }
//        }
//        #endregion Other
//    }
//}
