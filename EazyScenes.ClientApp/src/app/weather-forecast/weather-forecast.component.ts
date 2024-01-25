import {Component, OnInit} from '@angular/core';
import {environment} from "../../environments/environment";
import {WeatherForecastService} from "./weather-forecast.service";

@Component({
  selector: 'app-weather-forecast',
  templateUrl: './weather-forecast.component.html',
  styleUrl: './weather-forecast.component.css'
})
export class WeatherForecastComponent implements OnInit {

  constructor(private readonly weatherService: WeatherForecastService) {
  }

  ngOnInit(): void {
    this.weatherService.getWeatherData().subscribe((response) => {
      console.log("weather forecast endpoint gave me this ", response);
    })
  }


}
