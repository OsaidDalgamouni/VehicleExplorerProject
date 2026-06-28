export interface Make {
    make_ID: number;
    make_Name: string;
}

export interface VehicleType {
    vehicleTypeId: number;
    vehicleTypeName: string;
}

export interface Model {
    make_ID: number;
    make_Name: string;
    model_ID: number;
    model_Name: string;
}

export interface ResponseResult<T> {
    status: ResultStatus;
    errors?: string[];
    totalCount: number;
    data?: T;
}

export enum ResultStatus 
{
    Failed=0, 
    Success=1,
    SuccessWithWaringn=2,
}