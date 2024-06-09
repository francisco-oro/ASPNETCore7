import {Component, OnInit} from '@angular/core';
import {City} from "../models/city";
import {CitiesService} from "../services/cities.service";
import {FormArray, FormControl, FormGroup, RequiredValidator, Validators} from "@angular/forms";

@Component({
  selector: 'app-cities',
  templateUrl: './cities.component.html',
  styleUrl: './cities.component.css'
})
export class CitiesComponent implements OnInit {
  public cities: City[] = [];
  postCityForm: FormGroup;
  public isPostCityFormSubmitted: boolean = false;

  putCityForm: FormGroup;
  editCityID: string | null = null;

  constructor(private citiesService: CitiesService) {
    this.postCityForm = new FormGroup({
      cityName: new FormControl(null, [Validators.required])
    });

    this.putCityForm = new FormGroup({
      cities: new FormArray([])
    })
  }

  get putCityFormArray(): FormArray{
    return this.putCityForm.get("cities") as FormArray;
  }

  loadCities() {
    this.citiesService.getCities()
      .subscribe({
        next: (response: City[]) => {
          this.cities = response;

          this.cities.forEach(City => {
            this.putCityFormArray.push(new FormGroup({
              cityID: new FormControl(City.cityID, [Validators.required]),
              cityName: new FormControl({ value: City.cityName, disabled: true}, [Validators.required])
            }));
          })
        },

        error: (error: any) => {
          console.log(error);
        },

        complete: () => {
        }
      });

  }

  ngOnInit(): void {
    this.loadCities();
  }

  get postCity_CityNameControl(): any {
    return this.postCityForm.controls["cityName"];
  }

  postCitySubmitted() {
    this.isPostCityFormSubmitted = true;

    this.citiesService.postCity
    (this.postCityForm.value).subscribe({
      next:( value: City) => {
        console.log(value);

        // this.loadCities();
        this.cities.push(new City(value.cityID, value.cityName));

        this.postCityForm.reset();
        this.isPostCityFormSubmitted = false;
      },
      error: err => {
        console.log(err);
      },
      complete:() => {}
    });
  }

  // Executes when the clicks on 'Edit' button the for the particular city
  editClicked(city: City): void {
    this.editCityID = city.cityID;
  }

  // Executes when the clicks on 'Update' button after editing
  updateClicked(i: number): void {
    this.citiesService.putCity(this.putCityFormArray.controls[i].value).subscribe({
      next: (response: string) => {
        console.log(response);

        this.editCityID = null;

        this.putCityFormArray.controls[i].reset(this.putCityFormArray.controls[i].value);
      },
      error: (err:any) => {},
      complete: () => {},

    })
  }
}

