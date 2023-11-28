using RestServerTest.Repository;
using RestServerTest.Repository.Entities;

public class CustomerRepository : ICustomerRepository
{
    private readonly CustomerDbContext dbContext;
    private readonly List<Customer> sortedCustomers;

    public CustomerRepository(CustomerDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        this.sortedCustomers = dbContext.Customers.ToList();
        SortCustomers();
    }

    public void AddCustomers(List<Customer> newCustomers)
    {
        foreach (var newCustomer in newCustomers)
        {
            // Validate customer
            if (string.IsNullOrEmpty(newCustomer.FirstName)
                || string.IsNullOrEmpty(newCustomer.LastName)
                || newCustomer.Age <= 18
                || sortedCustomers.Exists(c => c.Id == newCustomer.Id))
            {
                throw new ArgumentException("Invalid customer data");
            }

            InsertCustomer(newCustomer);
        }

        // Save the changes to the database
        dbContext.SaveChanges();
    }

    public List<Customer> GetCustomers()
    {
        SortCustomers();
        return sortedCustomers;
    }

    private void InsertCustomer(Customer newCustomer)
    {
        int index = 0;

        // Find the correct position to insert the new customer
        while (index < sortedCustomers.Count &&
               (string.Compare(sortedCustomers[index].LastName, newCustomer.LastName,
                    StringComparison.OrdinalIgnoreCase) < 0 ||
                (string.Compare(sortedCustomers[index].LastName, newCustomer.LastName,
                     StringComparison.OrdinalIgnoreCase) == 0 &&
                 string.Compare(sortedCustomers[index].FirstName, newCustomer.FirstName,
                     StringComparison.OrdinalIgnoreCase) < 0)))
        {
            index++;
        }

        // Insert the new customer at the found position
        sortedCustomers.Insert(index, newCustomer);

        // Reorder the list without using List.Sort()
        for (int i = index - 1; i >= 0; i--)
        {
            if (string.Compare(sortedCustomers[i].LastName + sortedCustomers[i].FirstName,
                    sortedCustomers[i + 1].LastName + sortedCustomers[i + 1].FirstName,
                    StringComparison.OrdinalIgnoreCase) > 0)
            {
                // Swap the elements
                (sortedCustomers[i], sortedCustomers[i + 1]) = (sortedCustomers[i + 1], sortedCustomers[i]);
            }
            else
            {
                break;
            }
        }

        // Add the new customer to the DbSet
        dbContext.Customers.Add(newCustomer);
    }


    private void SortCustomers()
    {
        var sortedList = new List<Customer>();

        foreach (var newCustomer in sortedCustomers)
        {
            int index = 0;

            // Find the correct position to insert the new customer
            while (index < sortedList.Count &&
                   (string.Compare(sortedList[index].LastName, newCustomer.LastName,
                        StringComparison.OrdinalIgnoreCase) < 0 ||
                    (string.Compare(sortedList[index].LastName, newCustomer.LastName,
                         StringComparison.OrdinalIgnoreCase) == 0 &&
                     string.Compare(sortedList[index].FirstName, newCustomer.FirstName,
                         StringComparison.OrdinalIgnoreCase) < 0)))
            {
                index++;
            }

            // Insert the new customer at the found position
            sortedList.Insert(index, newCustomer);
        }

        // Update sortedCustomers with the sorted list
        sortedCustomers.Clear();
        sortedCustomers.AddRange(sortedList);
    }
}