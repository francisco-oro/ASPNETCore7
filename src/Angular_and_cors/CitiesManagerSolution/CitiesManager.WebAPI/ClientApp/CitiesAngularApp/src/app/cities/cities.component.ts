import {Component, OnInit} from '@angular/core';
import {City} from "../models/city";
import {CitiesService} from "../services/cities.service";
import {FormControl, FormGroup, RequiredValidator, Validators} from "@angular/forms";

@Component({
  selector: 'app-cities',
  templateUrl: './cities.component.html',
  styleUrl: './cities.component.css'
})
export class CitiesComponent implements OnInit {
  public cities: City[] = [];
  postCityForm: FormGroup;
  public isPostCityFormSubmitted: boolean = false;

  constructor(private citiesService: CitiesService) {
    this.postCityForm = new FormGroup({
      cityName: new FormControl(null, [Validators.required])
    });
  }

  loadCities() {
    this.citiesService.getCities()
      .subscribe({
        next: (response: City[]) => {
          this.cities = response;
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
    })
  }
}

