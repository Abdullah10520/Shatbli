import { Component, input, output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslatePipe } from '@ngx-translate/core';
import { DesignType } from '../../../../shared/models/design.model';

@Component({
    selector: 'app-design-selector',
    standalone: true,
    imports: [CommonModule, TranslatePipe],
    templateUrl: './design-selector.component.html',
    styleUrl: './design-selector.component.css'
})
export class DesignSelectorComponent {
    // المدخلات
    selectedType = input<DesignType>(DesignType.Ceramic);

    // المخرجات
    typeChanged = output<DesignType>();

    DesignType = DesignType;

    selectType(type: DesignType): void {
        this.typeChanged.emit(type);
    }
}
