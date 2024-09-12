using Model.Domain;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Model.DAL
{
	public class JsonRepository<T> : IRepository<T> where T : class, IDomainObject, new()
	{
		private static int autoId;

		private List<T> items;
		private string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "TodoItems.json");
		public JsonRepository()
		{
			LoadData();
			items ??= [];
			autoId = items.Count + 1;

		}

		public T Create(T item)
		{
			if (this.Read(item.Id) != null)
			{
				throw new RepositoryException(Strings.ResourceManager.GetString("RepositoryExceptionCreate") ?? "");

			}
			else
			{
				item.Id = autoId++;
				items.Add(item);
				this.SaveAll();
				return item;
			}
		}

		public void Delete(int id)
		{
			T? item = this.Read(id);
			if (item == null)
			{
				throw new RepositoryException(Strings.ResourceManager.GetString("RepositoryExceptionDelete") ?? "");
			}
			else
			{
				items.Remove(item);
				this.SaveAll();
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
					this.SaveAll();
					isInRepository = true;
					break;
				}
			}
			if (!isInRepository)
			{
				throw new RepositoryException(Strings.ResourceManager.GetString("RepositoryExceptionUpdate") ?? "");
			}
		}

		public void SaveAll(IEnumerable<T>? externalData = null)
		{
			if (externalData == null)
			{
				externalData = items;
			}
			try
			{
				string json = Serialize(externalData);
				File.WriteAllText(fileName, json);
			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message, ex);
			}
		}

		private static readonly JsonSerializerOptions s_writeOptions = new()
		{
			Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.All),
			WriteIndented = true
		};

		static string Serialize<K>(K value)
		{
			return JsonSerializer.Serialize(value, s_writeOptions);
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
	}
}
