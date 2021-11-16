
using System;
using CMS;
using CMS.DocumentEngine;
using OslerAlumni.Admin.Core.Modules;
using OslerAlumni.Core.Definitions;
using OslerAlumni.Core.Kentico.Models;

using BaseModule = ECA.Admin.Core.Modules.BaseModule;

[assembly: RegisterModule(typeof(EventsSortingModule))]

namespace OslerAlumni.Admin.Core.Modules
{
    public class EventsSortingModule : BaseModule
    {
        public EventsSortingModule() 
            : base($"{GlobalConstants.ModulePrefix}.{nameof(EventsSortingModule)}")
        { }

        /// <summary>
        /// The system executes the Init method of the CMSModuleLoader attributes when the application starts.
        /// </summary>
        protected override void OnInit()
        {
            base.OnInit();

            DocumentEvents.Insert.After += Document_Update_After;
            DocumentEvents.Update.After += Document_Update_After;
        }

        #region "Events"

        /// <summary>
        /// Handles the After event of the Document_Update control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DocumentEventArgs"/> instance containing the event data.</param>
        private void Document_Update_After(object sender, DocumentEventArgs e)
        {
            if (e.Node == null) return;

            switch (e.Node.ClassName)
            {
                case PageType_Event.CLASS_NAME:
                    UpdateEventSortOrder(e.Node as PageType_Event);
                    break;
            }
        }

        private void UpdateEventSortOrder(PageType_Event eventPage)
        {
            bool isWebinarOnDemand = eventPage.DeliveryMethodsEnum == DeliveryMethods.WebinarOnDemand;

            eventPage.SortOrder = isWebinarOnDemand ? 1 : 0;
            eventPage.SortDummyDateTimeTicks = isWebinarOnDemand
                ? (DateTime.MaxValue - eventPage.EndDate).Ticks
                : eventPage.EndDate.Ticks;

            eventPage.Update();
        }

        #endregion
    }
}
