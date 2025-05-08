namespace QuarterEstate.APIS.Errors
{
    public class ApiValidationErorrResponse:ApiErrorResponse
    {
        public IEnumerable<string> Erorrs{ get; set; }=new List<string>();
        public ApiValidationErorrResponse():base(400)
        {
            
        }
    }
}
