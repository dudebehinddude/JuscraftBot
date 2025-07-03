using Discord;
using Discord.Interactions;

namespace JuscraftBot.Preconditions
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
  class InDevelopmentAttribute : PreconditionAttribute
  {
    public override async Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context, ICommandInfo commandInfo, IServiceProvider services)
    {
      var application = await context.Client.GetApplicationInfoAsync().ConfigureAwait(false);
      var ownerId = application.Owner.Id;

      if (ownerId == context.User.Id)
      {
        return PreconditionResult.FromSuccess();
      }

      return PreconditionResult.FromError("This command is currently under development");
    }
  }
}