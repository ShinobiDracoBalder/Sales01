namespace Sales01.ViewModels
{
    using Sales01.Common.Models;
    using Sales01.Services;

    public class ProductItemViewModel : ProductRequest
    {
        #region Attibutes
        private ApiService apiService;
        #endregion

        #region Constructors
        public ProductItemViewModel()
        {
            this.apiService = new ApiService();
        }
        #endregion
    }
}
