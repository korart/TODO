using Model.Domain;

namespace Model.DAL
{
	public interface IRepository<T> where T : IDomainObject, new()
	{
		public T Create(T item);
		public IEnumerable<T> Read();
		public T? Read(int id);
		public void Update(T item);
		public void Delete(int id);
		public void SaveAll(IEnumerable<T>? items);
	}
}
