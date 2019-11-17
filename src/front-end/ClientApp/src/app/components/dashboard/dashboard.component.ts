import { Component, OnInit, ViewChild, ElementRef, HostListener, OnDestroy } from '@angular/core';
import { StickerService } from '../../services/sticker.service';
import { StickerResultModel } from '../../models/stickers/sticker-result-model';
import { StickerAddInputModel } from 'src/app/models/stickers/sticker-add-input-model';
import { StickerUpdateInputModel } from '../../models/stickers/sticker-update-input-model';
import { ToolbarServie } from 'src/app/services/toolbar.service';
import { BackgroundPicturesService } from '../../services/background-pictures.service';
import { BackgroundPicture } from '../../models/view/background-picture';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit, OnDestroy {
  isLoading = false;
  backgroundUrl: string;
  stickers: StickerResultModel[] = [];
  selectedStickerId?: number;
  @ViewChild('dashboard', { static: false }) dashboard: ElementRef<HTMLDivElement>;

  constructor(private _stickerService: StickerService,
    private _toolbarServie: ToolbarServie,
    private _backgroundPicturesService: BackgroundPicturesService) { }

  ngOnDestroy(): void {
    this._toolbarServie.componentButtons = [];
  }

  async ngOnInit(): Promise<void> {
    this.isLoading = true;
    this.stickers = [];
    this._toolbarServie.componentButtons = [{
      right: false,
      title: 'Create',
      visible: true,
      action: () => this.onCreate(),
      icon: 'add',
      url: undefined,
      forceShowText: true,
    }];

    try {
      let backgroundPictures: BackgroundPicture[];
      [this.stickers, backgroundPictures] = await Promise.all([
        this._stickerService.getAll().toPromise(),
        this._backgroundPicturesService.getBackgrounds()
      ]);

      this.selectedStickerId = this.stickers.length > 0 ? this.stickers[0].id : undefined;
      this.backgroundUrl = encodeURI(`/assets/backgrounds/${backgroundPictures[Math.floor(Math.random() * (backgroundPictures.length + 1))].file}`);
    } catch (error) {
      alert(error.error.message);
    } finally {
      this.isLoading = false;
    }
  }

  @HostListener('document:keydown', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    switch (event.key) {
      case 'd':
      case 'D':
      case 'Delete':
        if (this.selectedStickerId !== undefined) {
          this.onDelete();
        }
        break;

      case 'A':
      case 'a':
      case 'I':
      case 'i':
      case 'Insert':
        this.onCreate();
        break;

      case 'e':
      case 'E':
      case 'Enter':
        if (this.selectedStickerId !== undefined) {
          this.onEdit();
        }
        break;

      case 'c':
      case 'C':
        if (this.selectedStickerId !== undefined) {
          this.onChangeColor();
        }
        break;
    }
  }

  stickerTrackBy(index: number, item: StickerResultModel): number {
    return item.id;
  }

  async onCreate(): Promise<void> {
    var text = prompt('Please enter text', '');
    if (text == null || text == '') {
      return;
    }

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
      htmlColor: this.getDistinctColor(),
      text: text,
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

  private getDistinctColor(): string {
    const goodColors = [
      '#e6194b', '#3cb44b', '#ffe119', '#4363d8', '#f58231', '#911eb4', '#46f0f0', '#f032e6', '#bcf60c', '#fabebe', '#008080', '#e6beff', '#9a6324', '#fffac8', '#800000', '#aaffc3', '#808000', '#ffd8b1', '#000075', '#808080',
    ];

    const randomNum = Math.floor(Math.random() * (goodColors.length + 1));
    return goodColors[randomNum];
  }

  async onChangeColor(): Promise<void> {
    const currentItem = this.stickers.find(z => z.id === this.selectedStickerId);

    currentItem.htmlColor = this.getDistinctColor();

    const item: StickerUpdateInputModel = {
      htmlColor: currentItem.htmlColor,
      itemId: currentItem.id,
      x: currentItem.x,
      y: currentItem.y,
      text: currentItem.text,
    };

    await this._stickerService.update(item).toPromise();
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
    this.selectedStickerId = this.stickers.length === 0 ? undefined : this.stickers[this.stickers.length - 1].id;
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
