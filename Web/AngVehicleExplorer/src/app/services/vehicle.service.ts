import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Make, Model, ResponseResult, VehicleType } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class VehicleService {

   private readonly api =  `${environment.apiUrl}/vehicles`;

    constructor(private http:HttpClient)
    {
    }

   getMakes(search?: string): Observable<ResponseResult<Make[]>> {
     let params = new HttpParams();
    if (search) 
    {
      params = params.set('search', search);
    }
    return this.http.get<ResponseResult<Make[]>>(
      `${this.api}/getMakes`,
      { params });
  }
  getVehicleTypes(makeId: number): Observable<ResponseResult<VehicleType[]>> {
  return this.http.get<ResponseResult<VehicleType[]>>(
    `${this.api}/getVehicleTypesForMake/${makeId}`
  );
}
getModels(makeId: number, year: number,  vehicleType: string, pageNumber: number,  pageSize: number): Observable<ResponseResult<Model[]>>
 {

    return this.http.get<ResponseResult<Model[]>>
    (
        `${this.api}/getModels`,  { params: {makeId, year,vehicleType,pageNumber, pageSize} }
    );
 }
}
