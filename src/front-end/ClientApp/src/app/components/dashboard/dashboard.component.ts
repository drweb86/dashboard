import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { StickerService } from '../../services/sticker.service';
import { StickerResultModel } from '../../models/stickers/sticker-result-model';
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

  constructor(private _stickerService: StickerService) { }

  async ngOnInit(): Promise<void> {
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

  // drag and drop.
  // due to Firefox not supporting Drop we have custom implementation.
  private _dragOffset: { left: number, top: number } = { left: 0, top: 0 };
  private _isDragging = false;
  private _draggedElement: HTMLElement;
  private _draggedItem: StickerResultModel;

  mousedown($event, id: number): void {
    this._isDragging = true;
    this._draggedElement = document.getElementById(`sticker-item-${id}`);
    this._draggedItem = this.stickers.find(z => z.id === id);
    this._dragOffset = {
      left: this._draggedElement.offsetLeft - $event.clientX,
      top: this._draggedElement.offsetTop - $event.clientY,
    }
  }

  async mouseup(): Promise<void> {
    if (this._isDragging) {

      this._isDragging = false;

      // update in database
      const item: StickerUpdateInputModel = {
        htmlColor: this._draggedItem.htmlColor,
        itemId: this._draggedItem.id,
        x: this._draggedItem.x,
        y: this._draggedItem.y,
        text: this._draggedItem.text,
      };

      await this._stickerService.update(item).toPromise();
    }
  }

  mousemove($event): void {
    $event.preventDefault();

    if (this._isDragging) {
      var mousePosition = {
        x: $event.clientX,
        y: $event.clientY
      };

      this._draggedItem.x = mousePosition.x + this._dragOffset.left;
      this._draggedItem.y = mousePosition.y + this._dragOffset.top;
    }
  }
}
