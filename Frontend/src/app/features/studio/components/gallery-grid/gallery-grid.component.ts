import { Component, input, output, signal, OnInit, inject, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Design, DesignType } from '../../../../shared/models/design.model';
import { DesignService } from '../../../../Core/services/design.service';

@Component({
    selector: 'app-gallery-grid',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './gallery-grid.component.html',
    styleUrl: './gallery-grid.component.css'
})
export class GalleryGridComponent implements OnInit {
    private designService = inject(DesignService);

    // المدخلات
    designType = input<DesignType>(DesignType.Ceramic);

    // المخرجات
    designSelected = output<Design>();
    customImageSelected = output<File>();

    // الحالة
    designs = signal<Design[]>([]);
    selectedDesign = signal<Design | null>(null);
    isLoading = signal(false);
    showCustomUpload = signal(false);

    constructor() {
        // تحديث المعرض عند تغيير النوع
        effect(() => {
            const type = this.designType();
            this.filterDesigns(type);
        });
    }

    ngOnInit(): void {
        this.loadDesigns();
    }

    private loadDesigns(): void {
        this.isLoading.set(true);
        this.designService.getAllDesigns().subscribe({
            next: (designs) => {
                this.filterDesigns(this.designType());
                this.isLoading.set(false);
            },
            error: () => {
                this.isLoading.set(false);
            }
        });
    }

    private filterDesigns(type: DesignType): void {
        const allDesigns = this.designService.designsSig();
        this.designs.set(allDesigns.filter(d => d.type === type));
    }

    selectDesign(design: Design): void {
        this.selectedDesign.set(design);
        this.showCustomUpload.set(false);
        this.designSelected.emit(design);
    }

    toggleCustomUpload(): void {
        this.showCustomUpload.update(v => !v);
        this.selectedDesign.set(null);
    }

    onCustomImageSelect(event: Event): void {
        const input = event.target as HTMLInputElement;
        if (input.files && input.files.length > 0) {
            this.customImageSelected.emit(input.files[0]);
        }
    }
}
