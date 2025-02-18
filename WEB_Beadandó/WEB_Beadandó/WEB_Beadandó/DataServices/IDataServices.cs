namespace WEB_Beadandó.DataServices;
    internal interface IDataServices
    {
        public Task<IEnumerable<T>> GetList<T>(string cmd, object? qParam = null);
        public Task<T?> GetById<T>(int id) where T : class;
        Task<int> Execute(string cmd, object? qParam = null);
}
