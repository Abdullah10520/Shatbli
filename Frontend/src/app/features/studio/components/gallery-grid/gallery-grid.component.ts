import { Component, input, output, signal, OnInit, inject, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslatePipe } from '@ngx-translate/core';
import { Ceramic, DesignType } from '../../../../shared/models/design.model';
import { DesignService } from '../../../../Core/services/design.service';

@Component({
    selector: 'app-gallery-grid',
    standalone: true,
    imports: [CommonModule, TranslatePipe],
    templateUrl: './gallery-grid.component.html',
    styleUrl: './gallery-grid.component.css'
})
export class GalleryGridComponent implements OnInit {
    private designService = inject(DesignService);

    // المدخلات
    designType = input<DesignType>(DesignType.Ceramic);

    // المخرجات
    designSelected = output<Ceramic>();
    customImageSelected = output<File>();

    // الحالة
    items = signal<Ceramic[]>([]);
    selectedItem = signal<Ceramic | null>(null);
    isLoading = signal(false);
    showCustomUpload = signal(false);

    constructor() {
        // تحديث المعرض عند تغيير النوع
        effect(() => {
            const type = this.designType();
            this.loadItems(type);
        });
    }

    ngOnInit(): void {
        this.loadItems(this.designType());
    }

    private loadItems(type: DesignType): void {
        this.isLoading.set(true);
        this.selectedItem.set(null);

        if (type === DesignType.Ceramic) {
            this.designService.getAllCeramics().subscribe({
                next: () => {
                    this.items.set(this.designService.ceramicsSig());
                    this.isLoading.set(false);
                },
                error: () => {
                    this.isLoading.set(false);
                }
            });
        } else {
            this.designService.getAllPaints().subscribe({
                next: () => {
                    this.items.set(this.designService.paintsSig());
                    this.isLoading.set(false);
                },
                error: () => {
                    this.isLoading.set(false);
                }
            });
        }
    }

    selectItem(item: Ceramic): void {
        this.selectedItem.set(item);
        this.showCustomUpload.set(false);
        this.designSelected.emit(item);
    }

    toggleCustomUpload(): void {
        this.showCustomUpload.update(v => !v);
        this.selectedItem.set(null);
    }

    onCustomImageSelect(event: Event): void {
        const input = event.target as HTMLInputElement;
        if (input.files && input.files.length > 0) {
            this.customImageSelected.emit(input.files[0]);
            this.showCustomUpload.set(false);
        }
    }
}
