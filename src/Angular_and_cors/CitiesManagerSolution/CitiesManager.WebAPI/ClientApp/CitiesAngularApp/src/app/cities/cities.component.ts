import {Component, OnInit} from '@angular/core';
import {City} from "../models/city";
import {CitiesService} from "../services/cities.service";

@Component({
  selector: 'app-cities',
  templateUrl: './cities.component.html',
  styleUrl: './cities.component.css'
})
export class CitiesComponent implements OnInit{
  public cities: City[] = [];

  constructor(private citiesService: CitiesService) {
  }

  ngOnInit(): void {
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
}

