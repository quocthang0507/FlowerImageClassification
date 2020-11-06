using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlowerImageClassification.WebApp.Utilties
{
	public static class Extension
	{
		public static string IsSelected(this IHtmlHelper htmlHelper, string controllers, string actions, string cssClass = "active")
		=> IsThisPage(htmlHelper, controllers, actions) ? cssClass : string.Empty;

		/// <summary>
		/// Check that the current page belongs to an action of the controller
		/// </summary>
		/// <param name="htmlHelper"></param>
		/// <param name="controllers"></param>
		/// <param name="actions"></param>
		/// <returns></returns>
		public static bool IsThisPage(this IHtmlHelper htmlHelper, string controllers, string actions)
		{
			string currentAction = htmlHelper.ViewContext.RouteData.Values["action"] as string;
			string currentController = htmlHelper.ViewContext.RouteData.Values["controller"] as string;

			IEnumerable<string> acceptedActions = (actions ?? currentAction).Split(',');
			IEnumerable<string> acceptedControllers = (controllers ?? currentController).Split(',');

			return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController);
		}

		/// <summary>
		/// Check if the address is localhost
		/// </summary>
		/// <param name="htmlHelper"></param>
		/// <returns></returns>
		public static bool IsLocalHost(this IHtmlHelper htmlHelper)
		{
			return htmlHelper.ViewContext.HttpContext.Request.Host.Value.Contains("localhost");
		}

	}
}
