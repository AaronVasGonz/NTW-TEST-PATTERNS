using Models;


namespace Data.Repository
{
    namespace Data.Repository
    {
        /// <summary>
        /// Interface for Category repository that defines CRUD operations.
        /// </summary>
        public interface ICategoryRepository
        {
            /// <summary>
            /// Deletes a category asynchronously by its ID.
            /// </summary>
            /// <param name="id">The ID of the category to delete.</param>
            /// <returns>A boolean indicating whether the category was successfully deleted.</returns>
            Task<bool> DeleteCategoryAsync(int id);

            /// <summary>
            /// Retrieves all categories asynchronously.
            /// </summary>
            /// <returns>An enumerable list of categories.</returns>
            Task<IEnumerable<Category>> GetCategoriesAsync();

            /// <summary>
            /// Retrieves a category by its ID asynchronously.
            /// </summary>
            /// <param name="id">The ID of the category to retrieve.</param>
            /// <returns>The category object if found, otherwise null.</returns>
            Task<Category> GetCategoryByIdAsync(int id);


            /// <summary>
            /// Retrieves a category by its name asynchronously.
            /// </summary>
            /// <param name="name">The name of the category to retrieve.</param>
            /// <returns>The category object if found, otherwise null.</returns>
            Task<Category> GetCategoryByNameAsync(string name);

            /// <summary>
            /// Saves a category asynchronously. It performs an update if the category already exists, or creates a new one if not.
            /// </summary>
            /// <param name="category">The category to save.</param>
            /// <returns>The saved category object.</returns>
            Task<Category> SaveCategoryAsync(Category category);
        }

        /// <summary>
        /// Category repository that implements ICategoryRepository interface.
        /// Provides methods for CRUD operations on categories.
        /// </summary>
        public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
        {
            /// <summary>
            /// Retrieves all categories asynchronously.
            /// </summary>
            /// <returns>An enumerable list of all categories.</returns>
            public async Task<IEnumerable<Category>> GetCategoriesAsync()
            {
                // Retrieves all categories from the base repository
                return await ReadAsync();
            }

            /// <summary>
            /// Retrieves a category by its ID asynchronously.
            /// </summary>
            /// <param name="id">The ID of the category to retrieve.</param>
            /// <returns>The category object if found, otherwise null.</returns>
            public async Task<Category> GetCategoryByIdAsync(int id)
            {
                // Reads all categories and returns the first matching one by ID
                var categories = await ReadAsync();
                return categories.FirstOrDefault(c => c.CategoryId == id);
            }

            public async Task<Category> GetCategoryByNameAsync(string name)
            {
                // Reads all categories and returns the first matching one by name
                var categories = await ReadAsync();
                return categories.FirstOrDefault(c => c.CategoryName == name);
            }

            /// <summary>
            /// Saves a category asynchronously. If the category exists, it updates it, otherwise creates a new category.
            /// </summary>
            /// <param name="category">The category to save or update.</param>
            /// <returns>The saved or updated category object.</returns>
            public async Task<Category> SaveCategoryAsync(Category category)
            {
                // Checks if the category exists (i.e., if it has a valid CategoryId)
                var exists = category.CategoryId != null && category.CategoryId > 0;

                // If the category exists, update it, otherwise create a new one
                if (exists)
                    await UpdateAsync(category);
                else
                    await CreateAsync(category);

                // Retrieves the updated list of categories and returns the saved category by ID
                var updated = await ReadAsync();
                return updated.SingleOrDefault(x => x.CategoryId == category.CategoryId);
            }

            /// <summary>
            /// Deletes a category asynchronously by its ID.
            /// </summary>
            /// <param name="id">The ID of the category to delete.</param>
            /// <returns>A boolean indicating whether the category was successfully deleted.</returns>
            public async Task<bool> DeleteCategoryAsync(int id)
            {
                // Attempts to retrieve the category by its ID
                var category = await GetCategoryByIdAsync(id);
                if (category == null)
                    return false; // If the category does not exist, return false

                // Deletes the category using the base repository
                await DeleteAsync(category);
                return true; // Returns true indicating successful deletion
            }
        }
    }
}
