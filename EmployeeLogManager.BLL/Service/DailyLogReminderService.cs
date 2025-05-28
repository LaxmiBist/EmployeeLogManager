using EmployeeLogManager.BLL.ServiceInterfaces;
using EmployeeLogManager.DAL.Data;
using Microsoft.EntityFrameworkCore;

public class DailyLogReminderService
{
    private readonly EmployeeLogManagerDbcontext _context;
    private readonly IEmailSenderService _emailSenderService;

    public DailyLogReminderService(EmployeeLogManagerDbcontext context, IEmailSenderService emailSenderService)
    {
        _context = context;
        _emailSenderService = emailSenderService;
    }

    public async Task SendRemindersAsync()
    {
        // Get today's date UTC [without time].
        var today = DateTime.UtcNow.Date;

        // Get all users from the Users table.
        var allUsers = await _context.Users.ToListAsync();

        // Get the list of UserIds who have submitted logs today.
        var submittedUserIds = await _context.DailyLogs
            .Where(log => log.Date == today) // Filter logs to only include entries from today.
            .Select(log => log.UserId) // Select only the UserId field.
            .ToListAsync();

        //  filters allUsers to only include those whose Id is not in the submittedUserIds list.
        var usersToRemind = allUsers
            .Where(u => !submittedUserIds.Contains(u.Id)) // Check if the userId is not in the submittedUserIds list.
            .ToList();


        foreach (var user in usersToRemind)
        {
            await _emailSenderService.SendEmailAsync(
                user.Email,
                "Reminder: Daily Log Pending",          
                $"Hi {user.FullName},\n\nYou haven't submitted your daily log for {today:dd MMM yyyy}."// Body of email.
            );
        }

    }
}
