using Asset.Domain.Repositories;


namespace Asset.Domain
{
    public interface IUnitOfWork
    {
        int CommitAsync();
        void Rollback();
        IRoleCategoryRepository RoleCategory { get; }
        IOrganizationRepository OrganizationRepository { get; }
        ISubOrganizationRepository SubOrganizationRepository { get; }

        IGovernorateRepository GovernorateRepository { get; }
        ICityRepository CityRepository { get; }
        IHospitalRepository HospitalRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        IAssetPeriorityRepository AssetPeriorityRepository { get; }
        IMasterAssetRepository MasterAssetRepository { get; }
        IMasterAssetComponentRepository MasterAssetComponentRepository { get; }
        IAssetDetailRepository AssetDetailRepository { get; }
        IBrandRepository BrandRepository { get; }
        ICategoryTypeRepository CategoryTypeRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IOriginRepository OriginRepository { get; }
        ISubCategoryRepository SubCategoryRepository { get; }
        ISupplierRepository SupplierRepository { get; }

        ISupplierHoldReasonRepository SupplierHoldReasonRepository { get; }
        ISupplierExecludeReasonRepository SupplierExecludeReasonRepository { get; }
        ISupplierExecludeAssetRepository SupplierExecludeAssetRepository { get; }

        ISupplierExecludeRepository SupplierExecludeRepository{ get; }
        IHospitalReasonTransactionRepository HospitalReasonTransactionRepository { get; }

        IApplicationTypeRepository ApplicationTypeRepository { get; }
        IHospitalApplicationRepository HospitalApplicationRepository { get; }
        IHospitalExecludeReasonRepository HospitalExecludeReasonRepository { get; }
        IHospitalHoldReasonRepository HospitalHoldReasonRepository { get; }

        IHospitalSupplierStatusRepository HospitalSupplierStatusRepository { get; }
   
        IAssetMovementRepository AssetMovementRepository { get; }
        IECRIRepository ECRIRepository { get; }
        IAssetStatusRepository AssetStatusRepository { get; }
        IAssetStatusTransactionRepository AssetStatusTransactionRepository { get; }
        IContractDetailRepository ContractDetailRepository { get; }
        IMasterContractRepository MasterContractRepository { get; }
        IBuildingRepository BuildingRepository { get; }
        IFloorRepository FloorRepository { get; }
        IRoomRepository RoomRepository { get; }
        IClassificationRepository ClassificationRepository { get; }
        IAssetOwnerRepository AssetOwnerRepository { get; }
        IPMAssetTaskRepository PMAssetTaskRepository { get; }
        ICommetieeMemberRepository CommetieeMemberRepository { get; }
        IPMTimeRepository PMTimeRepository { get; }

        IPMAssetTimeRepository PMAssetTimeRepository { get; }

        IWNPMAssetTimeRepository WNPMAssetTimeRepository { get; }

        IRequestRepository Request { get; }
        IRequestPeriorityRepository RequestPeriority { get; }
        IRequestModeRepository RequestMode { get; }
        IRequestDocumentRepository RequestDocument { get; }
        IRequestTrackingRepository RequestTracking { get; }
        IRequestStatusRepository RequestStatus { get; }

        IRequestTypeRepository RequestType { get; }
        IProblemRepository Problem { get; }
        ISubProblemRepository SubProblem { get; }

        IAssetWorkOrderTaskRepository AssetWorkOrderTask { get; }
        IWorkOrderRepository WorkOrder { get; }
        IWorkOrderPeriorityRepository WorkOrderPeriority { get; }
        IWorkOrderStatusRepository WorkOrderStatus { get; }
        IWorkOrderTaskRepository WorkOrderTask { get; }
        IWorkOrderTypeRepository WorkOrderType { get; }
        IWorkOrderTrackingRepository WorkOrderTracking { get; }
        IWorkOrderAttachmentRepository WorkOrderAttachment { get; }
        IWorkOrderAssignRepository WorkOrderAssignRepository { get; }

        IPMAssetTaskScheduleRepository pMAssetTaskScheduleRepository { get; }
        IPagingRepository pagingRepository { get; }
        IGroupingRepository groupingRepository { get; }
        IHealthRepository healthRepository { get; }

        IVisitRepository visitRepository { get; }
        IVisitTypeRepository visitTypeRepository { get; }

        IEngineerRepository EngineerRepository { get; }

        ISettingRepository SettingRepository { get; }


        IScrapRepository scrapRepository { get; }
        IScrapReasonRepository scrapReasonRepository { get; }

        IExternalAssetMovementRepository ExternalAssetMovementRepository { get; }
        IExternalFixRepository ExternalFixRepository { get; }
        IAssetStockTakingRepository AssetStockTackingRepository { get; }
        IStockTakingScheduleRepository StockTakingScheduleRepository { get; }
        IStockTakingHospitalRepository StockTakingHospitalRepository { get; }


        IManufacturerPMAssetRepository ManufacturerPMAssetRepository { get; }



    }
}
