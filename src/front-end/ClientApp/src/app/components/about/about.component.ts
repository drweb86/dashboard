import { Component, OnInit } from '@angular/core';
import { BackgroundPicture } from '../../models/view/background-picture';
import { BackgroundPicturesService } from 'src/app/services/background-pictures.service';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent implements OnInit {
  backgroundPictures: BackgroundPicture[] = [];

  constructor(private _backgroundPicturesService: BackgroundPicturesService) { }

  async ngOnInit(): Promise<void> {
    this.backgroundPictures = await this._backgroundPicturesService.getBackgrounds();
  }
}

