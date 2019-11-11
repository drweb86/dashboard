import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { StickerService } from '../../services/sticker.service';
import { StickerResultModel } from '../../models/stickers/sticker-result-model';
import { Router } from '@angular/router';
import { StickerAddInputModel } from 'src/app/models/stickers/sticker-add-input-model';
import { StickerUpdateInputModel } from '../../models/stickers/sticker-update-input-model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  isLoading = false;
  stickers: StickerResultModel[] = [];
  selectedStickerId?: number;
  @ViewChild('dashboard', { static: false }) dashboard: ElementRef<HTMLDivElement>;

  constructor(private _stickerService: StickerService, private _router: Router) { }

  async ngOnInit(): Promise<void> {
    if (navigator.userAgent.search("Firefox") > -1) {
      alert('Firefox browser is not supported. They did not implement getting coordinates for DROP event https://bugzilla.mozilla.org/show_bug.cgi?id=505521. for 10 years.');
      await this._router.navigate(['/']);
    }


    this.isLoading = true;
    this.stickers = [];
    try {
      this.stickers = await this._stickerService.getAll().toPromise();
      this.selectedStickerId = this.stickers.length > 0 ? this.stickers[0].id : undefined;
    } catch (error) {
      alert(error.error.message);
    } finally {
      this.isLoading = false;
    }
  }

  stickerTrackBy(index: number, item: StickerResultModel): number {
    return item.id;
  }

  async onCreate(): Promise<void> {

    // Select position
    let cornerX = 50;
    let cornerY = 50;
    const step = 20;

    while (this.stickers.some(z => z.x === cornerX)) {
      cornerX += step;
    }

    while (this.stickers.some(z => z.y === cornerY)) {
      cornerY += step;
    }

    // Add item
    const newSticker: StickerAddInputModel = {
      htmlColor: 'orange',
      text: 'New Item',
      x: cornerX,
      y: cornerY,
    }

    const createdSticker = await this._stickerService.add(newSticker).toPromise();
    this.stickers = [
      ...this.stickers,
      createdSticker,
    ];

    // Select it.

    this.selectedStickerId = createdSticker.id;
  }

  async onEdit(): Promise<void> {
    const currentItem = this.stickers.find(z => z.id === this.selectedStickerId);

    var text = prompt('Please enter text', currentItem.text);

    if (!(text == null || text == '')) {
      currentItem.text = text;

      const item: StickerUpdateInputModel = {
        htmlColor: currentItem.htmlColor,
        itemId: currentItem.id,
        x: currentItem.x,
        y: currentItem.y,
        text: currentItem.text,
      };

      await this._stickerService.update(item).toPromise();
    }
  }

  async onDelete(): Promise<void> {
    await this._stickerService.delete(this.selectedStickerId).toPromise();
    this.stickers = this.stickers.filter(z => z.id !== this.selectedStickerId);
    this.selectedStickerId = this.stickers.length === 0 ? undefined : this.stickers[0].id;
  }

  onSelect(id: number): void {
    this.selectedStickerId = id;
  }

  dragStart(e: { screenX: number, screenY: number, pageX: number, pageY: number }, sticker: StickerResultModel): void {
    // const dashboardBoundRectangle = this.dashboard.nativeElement.getBoundingClientRect();
    // const top = dashboardBoundRectangle.top;
    // const left = dashboardBoundRectangle.left;
    // console.log(`Local Top = ${top}, Left = ${left}`);
    // console.log(`Global Top = ${top + window.scrollY}, Left = ${left + window.scrollX}`);
    // console.log(`onDragEnd X = ${e.screenX - left}, Y = ${e.screenY - top}`);
    console.log('Start ', e.pageX, e.pageY);
  }

  onDragEnd(e: { screenX: number, screenY: number, pageX: number, pageY: number }, sticker: StickerResultModel): void {
    // const dashboardBoundRectangle = this.dashboard.nativeElement.getBoundingClientRect();
    // const top = dashboardBoundRectangle.top;
    // const left = dashboardBoundRectangle.left;
    // console.log(`Local Top = ${top}, Left = ${left}`);
    // console.log(`Global Top = ${top + window.scrollY}, Left = ${left + window.scrollX}`);
    // console.log(`onDragEnd X = ${e.screenX - left}, Y = ${e.screenY - top}`);
    console.log('End ', e.pageX, e.pageY);
  }

  // https://stackoverflow.com/questions/42334722/drag-to-move-a-component-around-the-page

  onDragEnded(event) {
    console.log(event.clientX);
    console.log(event.clientY);

    // let element = event.source.getRootElement();
    // let boundingClientRect = element.getBoundingClientRect();
    // let parentPosition = this.getPosition(element);
    // console.log('x: ' + (boundingClientRect.x - parentPosition.left), 'y: ' + (boundingClientRect.y - parentPosition.top));
  }

  getPosition(el) {
    let x = 0;
    let y = 0;
    while (el && !isNaN(el.offsetLeft) && !isNaN(el.offsetTop)) {
      x += el.offsetLeft - el.scrollLeft;
      y += el.offsetTop - el.scrollTop;
      el = el.offsetParent;
    }
    return { top: y, left: x };
  }
}
