using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
namespace SportSite.Models.Db
{
    public class Db : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Training> Trainings { get; set; }
        public virtual DbSet<Coach> Coaches { get; set; }
        public virtual DbSet<Services> Services { get; set; }
        public virtual DbSet<CreateCodeAccounts> Code { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<DayOfWeekTraining> DayofWeekTrainings { get; set; }
        public Db(DbContextOptions<Db> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        
        public User GetUser(string Id)
        {
            return Users.FirstOrDefault(x => x.Id.ToString() == Id);
        }
        public void AddTypeSport()
        {
            var service = new Services()
            {
                Name = "CrossFit",
                IsTypeSport = true,
                Details = @"
                    <h2>What is CrossFit?</h2>
                    A form of high intensity interval training,
                    CrossFit is a strength and conditioning workout that is made up of functional movement performed at a high intensity level.
                    <br/>
                    These movements are actions that you perform in your day - to - day life, like squatting, pulling, pushing etc.Many workouts feature variations of squats,
                    push - ups, and weight lifting that last for predetermined amounts of time to help build muscles.This varies from a traditional workout that may tell you 
                    how many reps to do over any period of time.
                    <br/>
                    CrossFit Journal notes that the workouts are so effective because of their emphasis on the elements of load, distance and speed, which help participants develop 
                    high levels of power. The workout may utilize different equipment to accomplish this, including kettle bells, rowers and bikes, medicine balls, speed ropes, rings 
                    and plyo boxes.
                    <br/>
                    CrossFit is similar to Orange Theory in that there is a standard `workout of the day`(WOD) that all members complete on the same day. The daily workout can be found 
                    on their website(which is always free), along with a guide to all the specialized lingo that is used.There is also a substitutions section on their FAQ page that suggests 
                    places to find level appropriate workouts. “CrossFit is universally scalable and modifiable for all fitness levels, so it can be tailored to meet your goals and current
                    fitness level,” says Tracey Magee, owner and head coach of CrossFit Clan Performance Center.",
                Image = "/Images/1.jpg"
            };
            var service2 = new Services()
            {
                Name = "Yoga",
                IsTypeSport = true,
                Details = @"
                    <h2>What does Yoga mean?</h2>
                    Yoga is a physical, mental and spiritual practice that originated in ancient India. First codified by the sage Patanjali in his Yoga Sutras around 400 C.E, the practice 
                    was in fact handed down from teacher to student long before this text arose. Traditionally, this was a one-to-one transmission, but since yoga became popular in the West 
                    in the 20th century, group classes have become the norm.
                    <br/>
                    The word yoga is derived from the Sanskrit root yuj, meaning “to yoke,” or “to unite”. The practice aims to create union between body, mind and spirit, as well as between 
                    the individual self and universal consciousness. Such a union tends to neutralize ego-driven thoughts and behaviours, creating a sense of spiritual awakening.
                    <br/>
                    Yoga has been practiced for thousands of years, and whilst many different interpretations and styles have been developed, most tend to agree that the ultimate goal of yoga
                    is to achieve liberation from suffering. Although each school or tradition of yoga has its own emphasis and practices, most focus on bringing together body, mind and breath
                    as a means of altering energy or shifting consciousness.",
                Image = "/Images/woman-yoga.jpg"
            };
            var service3 = new Services()
            {
                Name = "GYM",
                IsTypeSport = true,
                Details = @"
                    The gym offers a unique opportunity to take care of your body and improve your health. In the arsenal there are the most popular and effective simulators that will 
                    help you get enviable forms.
                    <br/>
                    The hall is designed for general physical and strength training of professional athletes and just those who want to pump up and increase endurance. 
                    For visitors are available:
                    <ul>
                        <li>cardio equipment (tracks, steppers, exercise bikes);</li>
                        <li>weight block simulators;</li>
                        <li>simulators for individual muscle groups (abdomen, hips, back, etc.);</li>
                        <li>power simulators (dumbbells, benches, etc.).</li>
                    </ul>
                    <br/>
                     In our hall you can:
                    <ul>
                        <li>reduce weight;</li>
                        <li>gain muscle mass;</li>
                        <li>adjust the figure;</li>
                        <li>Eliminate problem areas and much more.</li>
                    </ul>
                    <br/>
                    We have created all conditions for comfortable training. In addition to modern simulators, the room is provided with an air conditioning system. To improve conditions,
                    eliminate discomfort and expectations, there is a limit on the number of students. During classes, athletes will be able to enjoy not only a useful pastime, but also the 
                    pleasant sound of their favorite tracks. We always listen to the wishes of our visitors and select the most suitable music for the gym.
                    <br/>
                    You can work out in the gym both independently and under the strict guidance of our highly qualified trainers. Many years of experience allows them to select the best 
                    option for exercises, depending on the goals that the client wants to achieve. Among the employees are prize-winners and winners of domestic and international bodybuilding 
                    competitions. Professionals in their field will help you adjust your daily routine and nutrition to achieve the desired result faster.",
                Image = "/Images/gym.jpg"
            };
            Services.Add(new Services()
            {
                Name = "Solariy",
                Details = @"
                Lorem ipsum, dolor sit amet consectetur adipisicing elit. Obcaecati debitis consequuntur, repellat nemo ab accusantium ullam ea perspiciatis! Molestiae cupiditate
                consequuntur vero ad dicta eligendi, autem aperiam? Voluptates, nesciunt sequi.",
                Image = "/Images/solariy.jpg"
            });
            Services.Add(new Services()
            {
                Name = "Phytobar",
                Details = @"
                Lorem ipsum, dolor sit amet consectetur adipisicing elit. Obcaecati debitis consequuntur, repellat nemo ab accusantium ullam ea perspiciatis! Molestiae cupiditate 
                consequuntur vero ad dicta eligendi, autem aperiam? Voluptates, nesciunt sequi.",
                Image = "/Images/phytobar.jpg"
            });
            Services.Add(new Services()
            {
                Name = "Massage",
                Details = @"
                Lorem ipsum, dolor sit amet consectetur adipisicing elit. Obcaecati debitis consequuntur, repellat nemo ab accusantium ullam ea perspiciatis! Molestiae cupiditate
                consequuntur vero ad dicta eligendi, autem aperiam? Voluptates, nesciunt sequi.",
                Image = "/Images/massage.jpg"
            });
            Accounts.Add(new Account()
            {
                Client = new User()
                {
                    Age = 35,
                    Email = "asd@asd.com",
                    Gender = Gender.Female,
                    Name = "test",
                    Surname = "test",
                    Tel = "+3802222"
                },
                Login = "manager",
                Password = "manager",
                Role = Role.manager
            });
            Accounts.Add(new Account()
            {
                Client = new User()
                {
                    Age = 35,
                    Email = "asd@asd.com",
                    Gender = Gender.Female,
                    Name = "admin",
                    Surname = "admin",
                    Tel = "+3802222"
                },
                Login = "admin",
                Password = "admin",
                Role = Role.admin
            });
            Coaches.Add(new Coach()
            {
                Account = new Account()
                {
                    Client = new User()
                    {
                        Age = 35,
                        Email = "beatrice.barnaby@example.com",
                        Gender = Gender.Male,
                        Name = "Beatrice",
                        Surname = "Barnaby",
                        Tel = "+3802222"
                    },
                    Login = "coach1",
                    Password = "coach1",
                    Role = Role.coach
                },
                typeSports = service,
                Details = @"Certified CrossFit Level 1 Trainer (СF-L1) Champion of Ukraine in powerlifting in 2013 in the 52.5 kg category"
            });
            Coaches.Add(new Coach()
            {
                Account = new Account()
                {
                    Client = new User()
                    {
                        Age = 40,
                        Email = "xavier.lima@example.com",
                        Gender = Gender.Male,
                        Name = "Xavier",
                        Surname = "Lima",
                        Tel = "+3802222"
                    },
                    Login = "coach2",
                    Password = "coach2",
                    Role = Role.coach
                },
                typeSports = service,
                Details = @"Honored Master of Sports in Weightlifting;
                            Two-time champion of Europe;
                            Bronze medalist of the world championship (sum of dual events)"
            });
            Coaches.Add(new Coach()
            {
                Account = new Account()
                {
                    Client = new User()
                    {
                        Age = 31,
                        Email = "luz.cortes@example.com",
                        Gender = Gender.Female,
                        Name = "Luz",
                        Surname = "Cortes",
                        Tel = "+3802222"
                    },
                    Login = "coach3",
                    Password = "coach3",
                    Role = Role.coach
                },
                typeSports = service,
                Details = @"Certified CrossFit Trainer Level 1 (CF-L1)
                            Coaching experience - 6 years."
            });
            Coaches.Add(new Coach()
            {
                Account = new Account()
                {
                    Client = new User()
                    {
                        Age = 40,
                        Email = "heiko.hubert@example.com",
                        Gender = Gender.Male,
                        Name = "Heiko",
                        Surname = "Hubert",
                        Tel = "+3802222"
                    },
                    Login = "coach4",
                    Password = "coach4",
                    Role = Role.coach
                },
                typeSports = service2,
                Details = @"Certified CrossFit Trainer Level 1 (CF-L1)
                            Coaching experience - 6 years."
            });
            Coaches.Add(new Coach()
            {
                Account = new Account()
                {
                    Client = new User()
                    {
                        Age = 32,
                        Email = "orimira.gorbach@example.com",
                        Gender = Gender.Female,
                        Name = "Orimira",
                        Surname = "Gorbach",
                        Tel = "+3802222"
                    },
                    Login = "coach5",
                    Password = "coach5",
                    Role = Role.coach
                },
                typeSports = service2,
                Details = @"Certified CrossFit Trainer Level 1 (CF-L1)
                            Coaching experience - 6 years."
            });
            Coaches.Add(new Coach()
            {
                Account = new Account()
                {
                    Client = new User()
                    {
                        Age = 29,
                        Email = "oya.balaban@example.com",
                        Gender = Gender.Female,
                        Name = "Oya",
                        Surname = "Balaban",
                        Tel = "+3802222"
                    },
                    Login = "coach6",
                    Password = "coach6",
                    Role = Role.coach
                },
                typeSports = service3,
                Details = @"Certified trainer"
            });
            Coaches.Add(new Coach()
            {
                Account = new Account()
                {
                    Client = new User()
                    {
                        Age = 29,
                        Email = "chloe.king@example.com",
                        Gender = Gender.Female,
                        Name = "Chloe",
                        Surname = "King",
                        Tel = "+3802222"
                    },
                    Login = "coach7",
                    Password = "coach7",
                    Role = Role.coach
                },
                typeSports = service3,
                Details = @"Certified trainer"
            });
            Coaches.Add(new Coach()
            {
                Account = new Account()
                {
                    Client = new User()
                    {
                        Age = 29,
                        Email = "kenan.fahri@example.com",
                        Gender = Gender.Male,
                        Name = "Kenan",
                        Surname = "Fahri",
                        Tel = "+3802222"
                    },
                    Login = "coach8",
                    Password = "coach8",
                    Role = Role.coach
                },
                typeSports = service3,
                Details = @"Certified trainer"
            });
            this.SaveChanges();
        }
    }
}
