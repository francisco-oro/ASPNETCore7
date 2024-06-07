import {Component, OnInit} from '@angular/core';
import {City} from "../models/city";
import {CitiesService} from "../services/cities.service";
import {resolveProvidersRequiringFactory} from "@angular/compiler-cli/src/ngtsc/annotations/common";

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
      .subscribe(
        (response: City[] => {}),
      );
  }
}

