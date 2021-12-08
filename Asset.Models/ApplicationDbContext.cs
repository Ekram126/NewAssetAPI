using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Asset.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<RoleCategory> RoleCategories { get; set; }
        public DbSet<ApplicationRole> ApplicationRole { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<SubOrganization> SubOrganizations { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<HospitalDepartment> HospitalDepartments { get; set; }
        public DbSet<MasterAsset> MasterAssets { get; set; }
        public DbSet<MasterAssetComponent> MasterAssetComponents { get; set; }
        public DbSet<AssetDetail> AssetDetails { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Origin> Origins { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<CommetieeMember> CommetieeMembers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<AssetPeriority> AssetPeriorities { get; set; }

        public DbSet<MasterAssetAttachment> MasterAssetAttachments { get; set; }
        public DbSet<AssetDetailAttachment> AssetDetailAttachments { get; set; }

        public DbSet<AssetMovement> AssetMovements { get; set; }
        public DbSet<ECRI> ECRIS { get; set; }

        public DbSet<AssetStatu> AssetStatus { get; set; }
        public DbSet<AssetStatusTransaction> AssetStatusTransactions { get; set; }
        public DbSet<MasterContract> MasterContracts { get; set; }
        public DbSet<ContractDetail> ContractDetails { get; set; }
        public DbSet<RequestTracking> RequestTracking { get; set; }
        public DbSet<Request> Request { get; set; }
        public DbSet<RequestPhase> RequestPhase { get; set; }
        public DbSet<Phase> Phase { get; set; }
        public DbSet<RequestMode> RequestMode { get; set; }
        public DbSet<RequestStatus> RequestStatus { get; set; }
        public DbSet<RequestPeriority> RequestPeriority { get; set; }
        public DbSet<RequestDocument> RequestDocument { get; set; }
        public DbSet<RequestType> RequestTypes { get; set; }
        public DbSet<SubProblem> SubProblems { get; set; }
        public DbSet<Problem> Problems { get; set; }

        public DbSet<WorkOrderAttachment> WorkOrderAttachments { get; set; }
        public DbSet<WorkOrderTracking> WorkOrderTrackings { get; set; }
        public DbSet<WorkOrderStatus> WorkOrderStatuses { get; set; }
        public DbSet<WorkOrderPeriority> WorkOrderPeriorities { get; set; }
        public DbSet<WorkOrderType> WorkOrderTypes { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<WorkOrderTask> WorkOrderTasks { get; set; }
        public DbSet<AssetWorkOrderTask> AssetWorkOrderTasks { get; set; }
        public DbSet<WorkOrderAssign> WorkOrderAssigns { get; set; }


        public DbSet<Building> Buildings { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<Room> Rooms { get; set; }

        public DbSet<Classification> Classifications { get; set; }

        public DbSet<AssetOwner> AssetOwners { get; set; }
        public DbSet<PMAssetTask> PMAssetTasks { get; set; }
        public DbSet<PMTime> PMTimes { get; set; }

        public DbSet<PMAssetTime> PMAssetTimes { get; set; }
        public DbSet<PMAssetTaskSchedule> PMAssetTaskSchedules { get; set; }


        public DbSet<SupplierExeclude> SupplierExecludes { get; set; }
        public DbSet<SupplierExecludeReason> SupplierExecludeReasons { get; set; }    
        public DbSet<SupplierExecludeAsset> SupplierExecludeAssets { get; set; }
        public DbSet<SupplierExecludeAttachment> SupplierExecludeAttachments { get; set; }


        public DbSet<ApplicationType> ApplicationTypes { get; set; }
        public DbSet<HospitalExecludeReason> HospitalExecludeReasons { get; set; }
        public DbSet<HospitalHoldReason> HospitalHoldReasons { get; set; }
        public DbSet<HospitalApplication> HospitalApplications { get; set; }
        public DbSet<HospitalApplicationAttachment> HospitalApplicationAttachments { get; set; }
        public DbSet<HospitalReasonTransaction> HospitalReasonTransactions { get; set; }


    }
}
