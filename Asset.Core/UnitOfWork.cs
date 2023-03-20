using System;
using Asset.Domain.Repositories;
using Asset.Models;
using Asset.Domain;
using Asset.Core.Repositories;
using Microsoft.AspNetCore.Identity;
using Contract.Core.Repositories;

namespace Asset.Core
{
    public class UnitOfWork : IUnitOfWork
    {

        private IRoleCategoryRepository _roleCategoryRepository;
        private IOrganizationRepository _organizationRepository;
        private ISubOrganizationRepository _subOrganizationRepository;
        private IGovernorateRepository _governorateRepository;
        private ICityRepository _cityRepository;
        private IHospitalRepository _hospitalRepository;
        private IDepartmentRepository _departmentRepository;
        private IAssetPeriorityRepository _assetPeriorityRepository;
        private IAssetDetailRepository _assetDetailRepository;
        private IBrandRepository _brandRepository;

        private ICategoryTypeRepository _categoryTypeRepository;
        private ICategoryRepository _categoryRepository;
        private IEmployeeRepository _employeeRepository;
        private IMasterAssetRepository _masterAssetRepository;
        private IMasterAssetComponentRepository _masterAssetComponentRepository;
        private IOriginRepository _originRepository;
        private ISubCategoryRepository _subCategoryRepository;
        private ISupplierRepository _supplierRepository;
        private IAssetMovementRepository _assetMovementRepository;
        private IECRIRepository _eCRIRepository;
        private IAssetStatusRepository _assetStatusRepository;
        private IAssetStatusTransactionRepository _assetStatusTransactionRepository;
        private IMasterContractRepository _masterContractRepository;
        private IContractDetailRepository _contractDetailRepository;
        private IBuildingRepository _buildingRepository;
        private IFloorRepository _floorRepository;
        private IRoomRepository _roomRepository;
        private IClassificationRepository _classificationRepository;
        private IAssetOwnerRepository _assetOwnerRepository;
        private IPMAssetTaskRepository _pmAssetTaskRepository;
        private IPMTimeRepository _pmTimeRepository;
        private IPMAssetTimeRepository _pmAssetTimeRepository;
        private IWNPMAssetTimeRepository _wnPMAssetTimeRepository;
        private IRequestRepository _requestRepository;
        private IPagingRepository _pagingRepository;
        private IGroupingRepository _groupingRepository;
        private RequestPeriorityRepository _requestPeriorityRepository;
        private RequestModeRepository _requestModeRepository;
        private RequestDocumentRepository _requestDocumentRepository;
        private RequestTrackingRepository _requestTrackingRepository;
        private RequestStatusRepository _requestStatusRepository;
        private ICommetieeMemberRepository _commetieeMemberRepository;

        private RequestTypeRepository _requestTypeRepository;
        private ProblemRepository _problemRepository;
        private SubProblemRepository _subProblemRepository;
        private AssetWorkOrderTaskRepository _assetWorkOrderTaskRepository;
        private WorkOrderRepository _workOrderRepository;
        private WorkOrderPeriorityRepository _workOrderPeriorityRepository;
        private WorkOrderStatusRepository _workOrderStatusRepository;
        private WorkOrderTaskRepository _workOrderTaskRepository;
        private WorkOrderTypeRepository _workOrderTypeRepository;
        private WorkOrderTrackingRepository _workOrderTrackingRepository;
        private WorkOrderAttachmentRepository _workOrderAttachmentRepository;
        private PMAssetTaskScheduleRepository _pMAssetTaskScheduleRepository;
        private WorkOrderAssignRepositories _workOrderAssignRepository;


        private SupplierHoldReasonRepositories _supplierHoldReasonRepositories;
        private SupplierExecludeReasonRepositories _supplierExecludeReasonRepositories;
        private SupplierExecludeAssetRepositories _supplierExecludeAssetRepositories;
        private HospitalSupplierStatusRepository _hospitalSupplierStatusRepository;


        private ApplicationTypeRepositories _applicationTypeRepositories;
        private HospitalExecludeReasonRepositories _hospitalExecludeReasonRepositories;
        private HospitalHoldReasonRepositories _hospitalHoldReasonRepositories;
        private HospitalApplicationRepositories _hospitalApplicationRepositories;

        private HospitalReasonTransactionRepositories _hospitalReasonTransactionRepositories;
        private ISupplierExecludeRepository _supplierExecludeRepository;
        private ApplicationDbContext _context;
        private IHealthRepository _healthRepository;

        private VisitRepository _visitRepository;
        private VisitTypeRepository _visitTypeRepository;
        private EngineerRepository _engineerRepository;


        private ISettingRepository _settingRepository;


        private ScrapRepository _scrapRepository;
        private ScrapReasonRepository _scrapReasonRepository;

        private ExternalAssetMovementRepositories _externalAssetMovementRepositories;
        private IExternalFixRepository _externalFix;

        private AssetStockTakingRepository _assetStockTakingRepository;
        private StockTakingScheduleRepository _stockTakingScheduleRepository;
        private StockTakingHospitalRepository _stockTakingHospitalRepository;
        private IManufacturerPMAssetRepository _manufacturerPMAssetRepository;


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }


        public int CommitAsync()
        {
            return _context.SaveChanges();
        }

        public void Rollback()
        {
            _context.Dispose();
        }

        public IRoleCategoryRepository RoleCategory => _roleCategoryRepository = _roleCategoryRepository ?? new RoleCategoryRepositories(_context);

        public IOrganizationRepository OrganizationRepository => _organizationRepository = _organizationRepository ?? new OrganizationRepositories(_context);

        public ISubOrganizationRepository SubOrganizationRepository => _subOrganizationRepository = _subOrganizationRepository ?? new SubOrganizationRepositories(_context);

        public IGovernorateRepository GovernorateRepository => _governorateRepository = _governorateRepository ?? new GovernorateRepositories(_context);

        public ICityRepository CityRepository => _cityRepository = _cityRepository ?? new CityRepositories(_context);

        public IHospitalRepository HospitalRepository => _hospitalRepository = _hospitalRepository ?? new HospitalRepositories(_context);

        public IDepartmentRepository DepartmentRepository => _departmentRepository = _departmentRepository ?? new DepartmentRepositories(_context);

        public IAssetPeriorityRepository AssetPeriorityRepository => _assetPeriorityRepository = _assetPeriorityRepository ?? new AssetPeriorityRepositories(_context);

        public IMasterAssetRepository MasterAssetRepository => _masterAssetRepository = _masterAssetRepository ?? new MasterAssetRepositories(_context);

        public IMasterAssetComponentRepository MasterAssetComponentRepository => _masterAssetComponentRepository = _masterAssetComponentRepository ?? new MasterAssetComponentRepositories(_context);


        public IAssetDetailRepository AssetDetailRepository => _assetDetailRepository = _assetDetailRepository ?? new AssetDetailRepositories(_context);

        public IBrandRepository BrandRepository => _brandRepository = _brandRepository ?? new BrandRepositories(_context);

        public ICategoryTypeRepository CategoryTypeRepository => _categoryTypeRepository = _categoryTypeRepository ?? new CategoryTypeRepositories(_context);



        public ICategoryRepository CategoryRepository => _categoryRepository = _categoryRepository ?? new CategoryRepositories(_context);

        public IEmployeeRepository EmployeeRepository => _employeeRepository = _employeeRepository ?? new EmployeeRepositories(_context);

        public IOriginRepository OriginRepository => _originRepository = _originRepository ?? new OriginRepositories(_context);

        public ISubCategoryRepository SubCategoryRepository => _subCategoryRepository = _subCategoryRepository ?? new SubCategoryRepositories(_context);

        public ISupplierRepository SupplierRepository => _supplierRepository = _supplierRepository ?? new SupplierRepositories(_context);

        public ICommetieeMemberRepository CommetieeMemberRepository => _commetieeMemberRepository = _commetieeMemberRepository ?? new CommetieeMemberRepositories(_context);



        public IAssetMovementRepository AssetMovementRepository => _assetMovementRepository = _assetMovementRepository ?? new AssetMovementRepositories(_context);

        public IECRIRepository ECRIRepository => _eCRIRepository = _eCRIRepository ?? new ECRIRepositories(_context);

        public IAssetStatusRepository AssetStatusRepository => _assetStatusRepository = _assetStatusRepository ?? new AssetStatusRepositories(_context);

        public IAssetStatusTransactionRepository AssetStatusTransactionRepository => _assetStatusTransactionRepository = _assetStatusTransactionRepository ?? new AssetStatusTransactionRepositories(_context);

        public IContractDetailRepository ContractDetailRepository => _contractDetailRepository = _contractDetailRepository ?? new ContractDetailRepositories(_context);
        public IMasterContractRepository MasterContractRepository => _masterContractRepository = _masterContractRepository ?? new MasterContractRepositories(_context);


        public IClassificationRepository ClassificationRepository => _classificationRepository = _classificationRepository ?? new ClassificationRepositories(_context);

        public IPMAssetTaskRepository PMAssetTaskRepository => _pmAssetTaskRepository = _pmAssetTaskRepository ?? new PMAssetTaskRepositories(_context);


        public IPMTimeRepository PMTimeRepository => _pmTimeRepository = _pmTimeRepository ?? new PMTimeRepositories(_context);
        public IPMAssetTimeRepository PMAssetTimeRepository => _pmAssetTimeRepository = _pmAssetTimeRepository ?? new PMAssetTimeRepositories(_context);

        public IWNPMAssetTimeRepository WNPMAssetTimeRepository => _wnPMAssetTimeRepository = _wnPMAssetTimeRepository ?? new WNPMAssetTimeRepositories(_context);


        public IRequestRepository Request => _requestRepository = _requestRepository ?? new RequestRepository(_context);

        public IRequestPeriorityRepository RequestPeriority => _requestPeriorityRepository = _requestPeriorityRepository ?? new RequestPeriorityRepository(_context);

        public IRequestModeRepository RequestMode => _requestModeRepository = _requestModeRepository ?? new RequestModeRepository(_context);

        public IRequestDocumentRepository RequestDocument => _requestDocumentRepository = _requestDocumentRepository ?? new RequestDocumentRepository(_context);

        public IRequestTrackingRepository RequestTracking => _requestTrackingRepository = _requestTrackingRepository ?? new RequestTrackingRepository(_context);

        public IRequestStatusRepository RequestStatus => _requestStatusRepository = _requestStatusRepository ?? new RequestStatusRepository(_context);


        public IRequestTypeRepository RequestType => _requestTypeRepository = _requestTypeRepository ?? new RequestTypeRepository(_context);

        public IProblemRepository Problem => _problemRepository = _problemRepository ?? new ProblemRepository(_context);

        public ISubProblemRepository SubProblem => _subProblemRepository = _subProblemRepository ?? new SubProblemRepository(_context);

        public IAssetWorkOrderTaskRepository AssetWorkOrderTask => _assetWorkOrderTaskRepository = _assetWorkOrderTaskRepository ?? new AssetWorkOrderTaskRepository(_context);

        public IWorkOrderRepository WorkOrder => _workOrderRepository = _workOrderRepository ?? new WorkOrderRepository(_context);

        public IWorkOrderPeriorityRepository WorkOrderPeriority => _workOrderPeriorityRepository = _workOrderPeriorityRepository ?? new WorkOrderPeriorityRepository(_context);

        public IWorkOrderStatusRepository WorkOrderStatus => _workOrderStatusRepository = _workOrderStatusRepository ?? new WorkOrderStatusRepository(_context);

        public IWorkOrderTaskRepository WorkOrderTask => _workOrderTaskRepository = _workOrderTaskRepository ?? new WorkOrderTaskRepository(_context);

        public IWorkOrderTypeRepository WorkOrderType => _workOrderTypeRepository = _workOrderTypeRepository ?? new WorkOrderTypeRepository(_context);

        public IWorkOrderTrackingRepository WorkOrderTracking => _workOrderTrackingRepository = _workOrderTrackingRepository ?? new WorkOrderTrackingRepository(_context);

        public IWorkOrderAttachmentRepository WorkOrderAttachment => _workOrderAttachmentRepository = _workOrderAttachmentRepository ?? new WorkOrderAttachmentRepository(_context);


        public IBuildingRepository BuildingRepository => _buildingRepository = _buildingRepository ?? new BuildingRepositories(_context);

        public IFloorRepository FloorRepository => _floorRepository = _floorRepository ?? new FloorRepositories(_context);

        public IRoomRepository RoomRepository => _roomRepository = _roomRepository ?? new RoomRepositories(_context);

        public IAssetOwnerRepository AssetOwnerRepository => _assetOwnerRepository = _assetOwnerRepository ?? new AssetOwnerRepositories(_context);

        public IWorkOrderAssignRepository WorkOrderAssignRepository => _workOrderAssignRepository = _workOrderAssignRepository ?? new WorkOrderAssignRepositories(_context);


        public IPMAssetTaskScheduleRepository pMAssetTaskScheduleRepository => _pMAssetTaskScheduleRepository = _pMAssetTaskScheduleRepository ?? new PMAssetTaskScheduleRepository(_context);

        public ISupplierHoldReasonRepository SupplierHoldReasonRepository => _supplierHoldReasonRepositories = _supplierHoldReasonRepositories ?? new SupplierHoldReasonRepositories(_context);


        public ISupplierExecludeReasonRepository SupplierExecludeReasonRepository => _supplierExecludeReasonRepositories = _supplierExecludeReasonRepositories ?? new SupplierExecludeReasonRepositories(_context);

        public ISupplierExecludeAssetRepository SupplierExecludeAssetRepository => _supplierExecludeAssetRepositories = _supplierExecludeAssetRepositories ?? new SupplierExecludeAssetRepositories(_context);



        public IApplicationTypeRepository ApplicationTypeRepository => _applicationTypeRepositories = _applicationTypeRepositories ?? new ApplicationTypeRepositories(_context);
        public IHospitalExecludeReasonRepository HospitalExecludeReasonRepository => _hospitalExecludeReasonRepositories = _hospitalExecludeReasonRepositories ?? new HospitalExecludeReasonRepositories(_context);
        public IHospitalHoldReasonRepository HospitalHoldReasonRepository => _hospitalHoldReasonRepositories = _hospitalHoldReasonRepositories ?? new HospitalHoldReasonRepositories(_context);
        public IHospitalApplicationRepository HospitalApplicationRepository => _hospitalApplicationRepositories = _hospitalApplicationRepositories ?? new HospitalApplicationRepositories(_context);


        public IHospitalSupplierStatusRepository HospitalSupplierStatusRepository => _hospitalSupplierStatusRepository = _hospitalSupplierStatusRepository ?? new HospitalSupplierStatusRepository(_context);


        public IPagingRepository pagingRepository => _pagingRepository = _pagingRepository ?? new PagingRepository(_context);

        public IGroupingRepository groupingRepository => _groupingRepository = _groupingRepository ?? new GroupingRepository(_context);

        public IHospitalReasonTransactionRepository HospitalReasonTransactionRepository => _hospitalReasonTransactionRepositories = _hospitalReasonTransactionRepositories ?? new HospitalReasonTransactionRepositories(_context);

        public ISupplierExecludeRepository SupplierExecludeRepository => _supplierExecludeRepository = _supplierExecludeRepository ?? new SupplierExecludeRepositories(_context);

        public IHealthRepository healthRepository => _healthRepository = _healthRepository ?? new HealthRepository(_context);


        public IVisitRepository visitRepository => _visitRepository = _visitRepository ?? new VisitRepository(_context);

        public IVisitTypeRepository visitTypeRepository => _visitTypeRepository = _visitTypeRepository ?? new VisitTypeRepository(_context);

        public IEngineerRepository EngineerRepository => _engineerRepository = _engineerRepository ?? new EngineerRepository(_context);


        public ISettingRepository SettingRepository => _settingRepository = _settingRepository ?? new SettingRepositories(_context);


        public IScrapRepository scrapRepository => _scrapRepository = _scrapRepository ?? new ScrapRepository(_context);

        public IScrapReasonRepository scrapReasonRepository => _scrapReasonRepository = _scrapReasonRepository ?? new ScrapReasonRepository(_context);

        //    public IExternalAssetMovementRepository externalAssetMovementRepository => _externalAssetMovementRepositories = _externalAssetMovementRepositories ?? new ExternalAssetMovementRepositories(_context);

        public IExternalAssetMovementRepository ExternalAssetMovementRepository => _externalAssetMovementRepositories = _externalAssetMovementRepositories ?? new ExternalAssetMovementRepositories(_context);


        public IExternalFixRepository ExternalFixRepository => _externalFix = _externalFix ?? new ExternalFixRepositories(_context);

         public IAssetStockTakingRepository AssetStockTackingRepository => _assetStockTakingRepository = _assetStockTakingRepository ?? new AssetStockTakingRepository(_context);
        public IStockTakingHospitalRepository StockTakingHospitalRepository => _stockTakingHospitalRepository = _stockTakingHospitalRepository ?? new StockTakingHospitalRepository(_context);

        public IStockTakingScheduleRepository StockTakingScheduleRepository => _stockTakingScheduleRepository = _stockTakingScheduleRepository ?? new StockTakingScheduleRepository(_context);

        public IManufacturerPMAssetRepository ManufacturerPMAssetRepository => _manufacturerPMAssetRepository = _manufacturerPMAssetRepository ?? new ManufacturerPMAssetRepository(_context);

    }
}

