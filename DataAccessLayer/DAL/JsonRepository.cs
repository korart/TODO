using Model.Domain;
using System.Text.Json;

namespace Model.DAL
{
	public class JsonRepository<T> : IRepository<T> where T : class, IDomainObject, new()
	{
		private List<T> items;
		private string fileName = "TodoItems.json";
		public JsonRepository()
		{
			LoadData();
			items ??= [];

		}

		public void Create(T item)
		{
			if (this.Read(item.Id) != null)
			{
				throw new RepositoryException(Strings.ResourceManager.GetString("RepositoryExceptionCreate") ?? "");

			}
			else
			{
				items.Add(item);
				this.SaveData();
			}
		}

		public void Delete(T item)
		{
			if (this.Read(item.Id) == null)
			{
				throw new RepositoryException(Strings.ResourceManager.GetString("RepositoryExceptionDelete") ?? "");
			}
			else
			{
				items.Remove(item);
				this.SaveData();
			}
		}

		public IEnumerable<T> Read()
		{
			return items;
		}

		public T? Read(int id)
		{
			return items.Where<T>(i => i.Id == id).FirstOrDefault<T>();
		}

		public void Update(T item)
		{
			bool isInRepository = false;
			foreach (T value in items)
			{
				if (value.Id == item.Id)
				{
					items.Remove(value);
					items.Add(item);
					this.SaveData();
					isInRepository = true;
					break;
				}
			}
			if (!isInRepository)
			{
				throw new RepositoryException(Strings.ResourceManager.GetString("RepositoryExceptionUpdate") ?? "");
			}
		}

		private void LoadData()
		{
			try
			{
				string json = File.ReadAllText(fileName);
				items = JsonSerializer.Deserialize<List<T>>(json) ?? [];
			}
			catch (FileNotFoundException)
			{
				items = [];
			}

		}

		private void SaveData()
		{
			string json = JsonSerializer.Serialize(items);
			File.WriteAllText(fileName, json);
		}
	}
}
