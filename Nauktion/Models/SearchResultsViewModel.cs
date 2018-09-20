using System.Collections.Generic;

namespace Nauktion.Models
{
    public class SearchResultsViewModel
    {
        public SearchViewModel SearchModel { get; set; }
        public List<AuktionBudViewModel> AuktionModel { get; set; }
    }
}