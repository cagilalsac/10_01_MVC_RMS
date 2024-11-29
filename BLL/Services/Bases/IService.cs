namespace BLL.Services.Bases
{
    // Abstract Classes or Interfaces Way 2: Generic interface
    // TEntity can be any initializable reference type such as User, Role or Resource,
    // TModel can be any initializable reference type such as UserModel, RoleModel or ResourceModel
    public interface IService<TEntity, TModel> where TEntity : class, new() where TModel : class, new()
    {
        // Method definitions for all services that will implement this interface
        public IQueryable<TModel> Query();
        public Service Create(TEntity record);
        public Service Update(TEntity record);
        public Service Delete(int id);
    }
}
