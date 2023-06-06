using Microsoft.EntityFrameworkCore;
namespace Platform.Models
{
    public class SeedData
    {
        private CalculationContext context;
        private ILogger<SeedData> logger;
        private static Dictionary<int, long> calculations = new Dictionary<int, long>(){
            {1,0},{2,1},{3,3},{4,6},{5,10},{6,15},{7,21},{8,28},{9,36},{10,45}
        
        };
        public SeedData(CalculationContext context,ILogger<SeedData> logger)
        {
            this.context=context;
            this.logger=logger;
        }
        public void SeedDatabase(){
            context.Database.Migrate();
            if(context.Calculations?.Count() == 0){
                logger.LogInformation("Preparing to seed Database");
                context.Calculations.AddRange(calculations.Select(item=>new Calculation{
                    Count=item.Key,
                    Result=item.Value
                }));
                context.SaveChanges();
                logger.LogInformation("Database seeded");

            }else{
                logger.LogInformation("Database already seeded");
            }

        }
    }
}