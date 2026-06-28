import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Make, Model, ResultStatus, VehicleType } from 'src/app/models/models';
import { VehicleService } from 'src/app/services/vehicle.service';

@Component({
  selector: 'app-vehicle-search',
  templateUrl: './vehicle-search.component.html',
  styleUrls: ['./vehicle-search.component.scss']
})
export class VehicleSearchComponent implements OnInit {

  form!: FormGroup;
  filteredMakes: Make[] = [];
  vehicleTypes: VehicleType[] = [];
  currentYear = new Date().getFullYear();
  pageNumber = 1;
  pageSize = 10;
  totalRecords = 0;
  models: Model[] = [];

  constructor(private fb: FormBuilder, private vehicleService: VehicleService)
  {

  }

  ngOnInit(): void 
  {
    this.createForm();
  }

  createForm() 
  {
    this.form = this.fb.group
      ({
        make: [null, Validators.required],
        vehicleType: [null, Validators.required],
        year: [new Date().getFullYear(), Validators.required]
      });
  }
  filterMakes(event: any): void {
    this.vehicleService.getMakes(event.query).subscribe({
      next: result => {
        if (result.status !== ResultStatus.Success) 
        {
          console.error(result.errors);
          this.filteredMakes = [];
          return;
        }
        this.filteredMakes = result.data ?? [];
      },
      error: err => {
        console.error(err);
        this.filteredMakes = [];
      }
    });
  }

  onMakeSelected(event: Make): void {
    const selectedMake = event.make_ID;

    this.models = [];
    this.vehicleTypes = [];

    this.form.patchValue({
      vehicleType: null
    });

    this.vehicleService.getVehicleTypes(selectedMake).subscribe({
      next: result => {
        if (result.status !== ResultStatus.Success) {
          console.error(result.errors);
          this.vehicleTypes = [];
          return;
        }

        this.vehicleTypes = result.data ?? [];
      },
      error: err => {
        console.error(err);
        this.vehicleTypes = [];
      }
    });
  }
    search(): void 
    {

      const make = this.form.value.make;
      const vehicleType = this.form.value.vehicleType;

      this.vehicleService.getModels(   make.make_ID,this.form.value.year, vehicleType.vehicleTypeName,this.pageNumber, this.pageSize)
        .subscribe({
         next: result => 
          {
              if (result.status !== ResultStatus.Success) {
                console.error(result.errors);
                this.models = [];
                this.totalRecords = 0;
                return;
              }
              this.models = result.data ?? [];
              this.totalRecords = result.totalCount;
          },

          error: err => {
            console.error(err);
            this.models = [];
            this.totalRecords = 0;
          }
        });
    }

  changePage(page: number): void
  {

      if (page < 1)
          return;
      this.pageNumber = page;

      this.search();
  }
  pageSizeChanged(event: Event): void 
  {

      const target = event.target as HTMLSelectElement;
      this.pageSize = Number(target.value);
      this.pageNumber = 1;
      this.search();
  }
  get totalPages(): number 
  {

      return Math.ceil(this.totalRecords / this.pageSize);

  }

  get showingFrom(): number 
  {
    return this.totalRecords === 0 ? 0 : ((this.pageNumber - 1) * this.pageSize) + 1;
  }

  get showingTo(): number 
  {
    return Math.min(this.pageNumber * this.pageSize, this.totalRecords);
  }

}
