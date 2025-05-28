//using EmployeeLogManager.BLL.ServiceInterfaces; // commit msg: (commented out) Needed to access the IEmailService interface
//using Hangfire; // commit msg: (commented out) Needed to use Hangfire background job system

//using EmployeeLogManager.BLL.ServiceInterfaces;
//using Hangfire;

//public class JobTestService : IJobTestService // commit msg: Declares a class that schedules Hangfire jobs
//{
//    private readonly IRecurringJobManager _recurringJobManager; // commit msg: Injects the Hangfire job manager
//    private readonly IEmailService _emailService; // commit msg: Injects the application's email service

//    public JobTestService(IRecurringJobManager recurringJobManager, IEmailService emailService) // commit msg: Constructor with DI for dependencies
//    {
//        _recurringJobManager = recurringJobManager; // commit msg: Assigns recurring job manager
//        _emailService = emailService; // commit msg: Assigns email service
//    }

//    public void ScheduleJobs() // commit msg: Registers the recurring email reminder job
//    {
//        _recurringJobManager.AddOrUpdate( // commit msg: Adds or updates a recurring job in Hangfire
//            "daily-log-reminder", // commit msg: Job ID used by Hangfire
//            () => SendEmailReminder(), // commit msg: Lambda that calls a local method (must be public and parameterless)
//            "0 17 * * *" // commit msg: Cron expression for 5:00 PM daily
//        );
//    }

//    public void SendEmailReminder() // commit msg: This method will be called by Hangfire to send the email
//    {
//        _emailService.SendTestEmail("Reminder: Please fill out your daily log."); // commit msg: Sends reminder email using injected service
//    }
//}

//NOTE:
//the logic is slightly different because JobTestService is handling the task directly by 
//    calling its own method SendEmailReminder(). 
//    This method sends an email reminder directly, without delegating the job to another service.

//JobTestService handles the sending of the reminder itself by directly calling SendEmailReminder().





using EmployeeLogManager.BLL.ServiceInterfaces;
using Hangfire;

//only sheadule the job don't know about how to send the email.
public class JobTestService : IJobTestService
{
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly IEmailSenderService _emailSenderService;

    public JobTestService(IRecurringJobManager recurringJobManager, IEmailSenderService emailSenderService) 
    {
        _recurringJobManager = recurringJobManager;
        _emailSenderService = emailSenderService;
    }

    public void ScheduleJobs() 
    {
        //every day at 6:00 PM, use the DailyLogReminderService, and run its SendRemindersAsync() method.”
        RecurringJob.AddOrUpdate<DailyLogReminderService>(
            "daily-log-reminder",//unique id
            service => service.SendRemindersAsync(), // Lambda to call the reminder method on the service
            Cron.Daily(11, 5)
        );
    }
}


//Note
//JobTestService delegates the responsibility of sending reminders to DailyLogReminderService.
// CRON expression represents a time unit, in this order:
// ┌───────────── Minute(0 - 59)
// │ ┌───────────── Hour(0 - 23)
// │ │  ┌───────────── Day of month (1 - 31)
// │ │  │ ┌───────────── Month (1 - 12)
// │ │  │ │ ┌───────────── Day of week (0 - 6) (Sunday = 0 or 7)
// │ │  │ │ │a
// 0 17 * * * 